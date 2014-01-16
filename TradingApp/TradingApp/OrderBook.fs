namespace RiskLib

module OrderBook = 

    type OrderSide =
            Bid | Ask | Sellshort

    type Order = 
        {
            side : OrderSide
            price : decimal
            volume : decimal
            id :  int   
            symbol : string     
        }

    type OrderBook(symbol:string) =

        let splitOrder(order, newVolume) =
            {
                side = order.side
                price = order.price
                volume = newVolume
                id = order.id
                symbol = order.symbol
            }

        member val Model = obj() with get, set

        member val bidSide = [] with get, set
        member val askSide = [] with get, set
        member val Symbol = symbol with get, set

        member public this.addOrder(order: Order) =
            match order.side with 
                | Bid ->  this.bidSide <- order :: this.bidSide |> List.sortBy (fun a -> - a.price - 1.M)
                | _ -> this.askSide <- order :: this.askSide |> List.sortBy (fun a -> a.price) 

        member public this.trade(order,order2) = ()//printfn "Trade actioned %A,%A" order order2
    
        member public this.printOrderBook() = 
                List.iter (fun a -> printfn "%A,%A" a.id a.side) this.bidSide 

        member private this.split(order, newVolume) = splitOrder(order,newVolume)

        member public this.matchOrders() = 

                if not this.bidSide.IsEmpty && not this.askSide.IsEmpty then

                    let topOfBook = (this.bidSide.Head,this.askSide.Head)
                
                    match topOfBook with 
                        // price is too low
                        | (bid,ask) when bid.price < ask.price -> ()

                        // price is ok and volumes are exactly the same
                        | (bid,ask) when bid.price >= ask.price && bid.volume = ask.volume -> 
                            this.trade(bid,ask)
                            this.bidSide <- this.bidSide.Tail
                            this.askSide <- this.askSide.Tail
                            this.matchOrders()
                    
                        // price ok therefore cross orders
                        | (bid,ask) when bid.price >= ask.price && bid.volume < ask.volume ->
                            let matchingAsk = this.split(bid,bid.volume)
                            let remainingAsk = this.split(ask,ask.volume - bid.volume)
                            this.trade(bid, matchingAsk)
                            this.bidSide <- this.bidSide.Tail
                            this.askSide <- remainingAsk :: this.askSide.Tail
                            this.matchOrders()

                        | (bid,ask) when bid.price >= ask.price && bid.volume > ask.volume ->
                            let matchingBid = this.split(bid,ask.volume)
                            let remainingBid = this.split(bid,bid.volume - ask.volume)
                            this.trade(matchingBid,ask)
                            this.bidSide <- remainingBid :: this.bidSide.Tail
                            this.askSide <- this.askSide.Tail
                            this.matchOrders()
                        | _ -> ()
                else
                    ()


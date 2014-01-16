namespace RiskLib   

module MatchingEngine =

    open OrderBook

    type TxLog() =
        member public this.storeTx(order:Order) = () //printfn "%A" order
        member public this.close() = printfn "close TxLog"

    type MatchingEngine(id, orderbooks : OrderBook list) = 

            let matchOrder' map order = 0

            let log = new TxLog()

            member val StandBy : Option<MatchingEngine> = None with get, set
        
            member val txLog = log

            member val id = id

            member val Orderbooks = orderbooks with get, set
                    
            member val SupportedSymbols = 
                List.map (fun (a:OrderBook)-> a.Symbol) orderbooks
                with get

            member val orderbooksMap = 
                Map.ofList [ for i in orderbooks -> (i.Symbol,i) ] 
       
            member public this.close() = log.close()

            member public this.matchOrder(order:Order) : string =
                match this.orderbooksMap.TryFind(order.symbol) with
                    | Some(book) ->
                        this.txLog.storeTx order
                        book.addOrder(order)
                        book.matchOrders()

                        let standby = 
                             Option.map (fun (a: MatchingEngine) -> a.matchOrder(order)) this.StandBy
                        "placed order"
                    | _ -> "not supported"


    type MatchingEngineInfo =
        {
            primary : MatchingEngine
            standby : MatchingEngine Option 
            orderbooks : OrderBook list
        }
namespace RiskLib

module OrderReceiver = 
    open OrderBook
    open MatchingEngine

    type OrderReceiver() =
        member val matchingEngineForOrderbook : Map<string,MatchingEngine> = Map.empty with  get,set

        //construct
        member public this.refreshMatchingEnginePartitions(engines: MatchingEngine list) = 
                this.matchingEngineForOrderbook <-
                  [ for engine in engines do 
                    for symbol in engine.SupportedSymbols do
                    yield symbol, engine ] 
                  |> Map.ofSeq
            
        member public this.placeOrder(order:Order) : string =
                match this.matchingEngineForOrderbook.TryFind(order.symbol) with
                    | Some(me) -> me.matchOrder(order)
                    | None -> failwith "Unknown order book"
namespace RiskLib

module TradingSystem = 

    open System
    open OrderBook
    open MatchingEngine
    open OrderReceiver
   
    type TradingSystem() =

        let prefix = "A" :: "B" :: "C" :: "D" :: "E" :: "F" :: "G" :: "H" :: "I" :: ["J"]

        let groupedByMatchingEngine = 
            [ for i in prefix do
                for j in 1..10 do
                yield i + j.ToString()]  
            |> Seq.groupBy(fun a -> a.Chars(0))
            |> Seq.map(fun (a,b) -> b |>Seq.toList)
            |> Seq.toList
                        
        let createMatchingEngine id symbols = new MatchingEngine("ME" + id.ToString(),symbols)

        member val OR = [] with get,set
        member val ME = [] with get,set
        member val useStandby = true

        member public this.createOrderReceivers() = 
            this.OR <- [ for i in 1..10 -> new OrderReceiver()]

        member public this.orderbooksGroupedByMatchingEngine() =
            [for i in groupedByMatchingEngine -> 
                List.map (fun a -> OrderBook(a)) i] 

        member public this.createMatchingEngines() = 
            this.ME <-
                [ for a in this.orderbooksGroupedByMatchingEngine() do
                    let standByOption = 
                        if this.useStandby then
                            Some(createMatchingEngine (a|> Seq.head) a)
                        else
                            None
                    yield {
                            primary = createMatchingEngine (a|> Seq.head)  a
                            standby =  standByOption
                            orderbooks =  a }
                ]

        member public this.MatchingEngineRouting()= 
             [ for me in this.ME do 
                  for ob in me.orderbooks do
                    yield (me.primary)] //, ob.Symbol)] //|> Map.ofSeq
                    
        member public this.Start()= 
            
                // matching engines
                this.createMatchingEngines()
            
                // order riecvers
                this.createOrderReceivers()
                for mer in this.MatchingEngineRouting() do     
                    for orderRec in this.OR do
                        // set order recievers with matching engines
                        orderRec.refreshMatchingEnginePartitions(this.MatchingEngineRouting())





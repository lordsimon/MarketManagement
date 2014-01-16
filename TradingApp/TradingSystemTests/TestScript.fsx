
#r @"..\TradingApp\bin\Debug\TradingApp.dll"

open System
open RiskLib.OrderBook
open RiskLib.TradingSystem

let rnd = System.Random()
let newRnd (minNum,maxNum) = rnd.Next(minNum, maxNum)
let rndSide() = match rnd.NextDouble() with | n when n < 0.5 -> Bid | _ -> Ask  
let prefix = "A" :: "B" :: "C" :: "D" :: "E" :: "F" :: "G" :: "H" :: "I" :: ["J"]
let rndSymbol() = prefix.[newRnd(0,prefix.Length-1)] + [|1..prefix.Length|].[newRnd(0,prefix.Length-1)].ToString()
    
let randomOrder() = 
    {
        id = newRnd(0,1000000) 
        symbol = rndSymbol()
        price = (decimal)(newRnd(1,1000))
        volume = (decimal)(newRnd(10,1000))
        side = rndSide()
    }


let ts = new TradingSystem()
ts.Start()
let placeOrder() = printfn "%A" (ts.OR.Head.placeOrder(randomOrder()))

for i in 1..10000 do
    placeOrder()

let printOrderBook (orders: OrderBook) = 
    let pOrder = List.iter(fun a-> printfn "%A|%A|%A|%A|%A" a.id a.side a.price a.volume a.symbol)
    if not orders.bidSide.IsEmpty || not orders.askSide.IsEmpty then 
        printfn "Orderbook %A" orders.Symbol 
        orders.bidSide |> pOrder 
        orders.askSide |> pOrder 

let printMatchingEngines() =
    for me in ts.ME do
    for ob in me.orderbooks do
    printOrderBook(ob)

printMatchingEngines()

//printfn "%A" (orderRec.placeOrder(randomOrder()))
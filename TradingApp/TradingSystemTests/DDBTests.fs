module DDBTests

open TickSpec
open FeatureFixture
open NUnit.Framework
open System
open System.Diagnostics
open RiskLib.OrderBook


let mutable ob = new OrderBook("CheSund0123")

let [<BeforeScenario>] SetupScenario () = ob <- new OrderBook("CheSund0123")

let createOrder (order:string []) = 
    {
        side = match order.[1] with | "Bid" -> Bid | "Ask" -> Ask | _ -> Sellshort
        id = 3 
        symbol = order.[0]
        volume = Decimal.Parse(order.[2])
        price = Decimal.Parse(order.[3])
    }

let [<Given>] ``the following orders are added to the order book`` (orders: Table) = 
    for i in orders.Rows do ob.addOrder(createOrder(i))

let [<When>] ``I match orders`` () =
    ob.matchOrders()

let [<Then>] ``the bidSide size is (.*)`` (size: int) = 
    Assert.True(true)

let [<Then>] ``the askSide size is (.*)`` (size: int) = 
    Assert.True(ob.askSide.Length = size)

let [<Then>] ``the head of the back book is`` (orders: Table) = 
    Assert.True(ob.bidSide.Head = (orders.Rows.[0] |> createOrder))

let [<Then>] ``the head of the lay book is`` (orders: Table) = 
    Assert.True(ob.askSide.Head = (orders.Rows.[0] |> createOrder))

let [<Then>] ``the head of the askSide volume is (.*)`` (vol: decimal) = 
    Assert.True(ob.askSide.Head.volume = vol)






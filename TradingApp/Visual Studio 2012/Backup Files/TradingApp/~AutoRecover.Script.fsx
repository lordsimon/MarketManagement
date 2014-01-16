
#load "Library1.fs"
open TradingSystem

let genRandomNumber() =
    let rnd = new System.Random()
    int (rnd.Next(1, 100000))

let randomOrder id= {side = Buy ; id = id ; symbol = "";price = 1.4M ; volume = 23.8M}

let ts = new TradingSystem()
ts.createOrderRecivers()

let client (orderReciever : OrderReciever, orders : Order list) = 
        [ for i in 1..10 do
            for o in orders do
                // time this
                let resp = orderReciever.placeOrder(o)
                yield resp
                // 
        ]   



let a = [ for i in 1..10 -> i % 10]


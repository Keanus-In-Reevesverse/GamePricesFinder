const log = console.log;
const fetch = require('node-fetch');
const nuuvem = require('nuuvem');

const uri = "https://localhost:7265/sendGameList/";

const https = require("https");
const agent = new https.Agent({
    rejectUnauthorized: false
});

let gameList = [];

gameList = nuuvem.checkPrice("for honor").then(function (gameDataArray) {
    let results = [];
    gameDataArray.forEach(function (e) {
        var game = { title: e.title, price: e.price, currency: e.currency };
        results.push(game);
    });

    var json_text = JSON.stringify(results, null, 2);

    var request = encodeURI(uri + json_text);

    fetch(request, { agent }).then(function (response) {
        return response.json();
    })
        .then(function (jsonResponse) {
            // console.log(jsonResponse);
        });
});
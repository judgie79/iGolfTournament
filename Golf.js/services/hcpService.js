module.exports.getHcp = function (player, callback) {

    if(player.membership.clubNr === "" || player.membership.nr === "" || player.membership.serviceNr === "")
    {
        callback(false);
    }
    
    var hcpServer = "http://www.golf.de/publish/turnierkalender/handicap-abfrage/abfrage";
    var request = require('request');

    // Set the headers
    var headers = {
        'User-Agent': 'Super Agent/0.0.1',
        'Content-Type': 'application/x-www-form-urlencoded'
    }

    // Configure the request
    var options = {
        url: hcpServer,
        method: 'GET',
        headers: headers,
        qs: { 'clubNr': player.membership.clubNr, 'ausweisNr': player.membership.nr, 'serviceNr': player.membership.serviceNr }
    }

    // Start the request
    request(options, function (error, response, body) {
        if (!error && response.statusCode == 200) {
            // Print out the response body
            console.log(body)

            var cheerio = require('cheerio');
            var $ = cheerio.load(body);

            var hcp = Number($('.hcp .hcp_wrapper > .hcp_text_container').text().trim().replace(',', '.'));
            callback(hcp); 
            
        }
    });
}
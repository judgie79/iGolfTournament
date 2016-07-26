
var Promise = require('promise');

var JaySchema = require('jayschema');
var js = new JaySchema();

var playerSchema = require('../schemas/player.js');

var hcpService = require('../services/hcpService');

var Validator = function (player) {
    this.player = player;
};


Validator.prototype.validateHcp = function(){
    var me = this;
    return new Promise(function (resolve, reject) {
        
        if (player.membership) {
            hcpService.getHcp(player, function(hcp) {
                if (hcp) {
                    player.hcp = hcp;
                    player.isOfficialHcp = true;
                } else {
                    player.isOfficialHcp = false;
                    player.hcp = Number(player.hcp);
                }

                resolve(player);
            });
        }
        else {
            player.hcp = Number(player.hcp);
            player.isOfficialHcp = false;

            resolve(player);
        }
    });
};

Validator.prototype.validateSchema = function(){
    var me = this;
    return new Promise(function (resolve, reject) {

        //js.register(addressSchema);

        var isValid = js.validate(me.player, playerSchema);

        if (isValid.length == 0) {
            resolve();
        } else {
            reject({ reason: "Player is not compatible to schema", message: "Player is not compatible to schema", validationResult: isValid });
        }
    });
};

module.exports = Validator;
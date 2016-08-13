
var Promise = require('promise');

var JaySchema = require('jayschema');
var js = new JaySchema();

var playerSchema = require('../schemas/player.js');
var addressSchema =  require('../schemas/address.js');
var hcpService = require('../services/hcpService');

var Validator = function (player) {
    this.player = player;
};


Validator.prototype.validateHcp = function(){
    var me = this;
    return new Promise(function (resolve, reject) {
        
        if (me.player.membership && !me.player.overrideHcp) {
            hcpService.getHcp(me.player, function(hcp) {
                if (hcp) {
                    me.player.hcp = hcp;
                    me.player.isOfficialHcp = true;
                } else {
                    me.player.isOfficialHcp = false;
                    me.player.hcp = Number(player.hcp);
                }

                resolve(me.player);
            });
        }
        else {
            me.player.hcp = Number(me.player.hcp);
            me.player.isOfficialHcp = false;

            resolve(me.player);
        }
    });
};

Validator.prototype.validateSchema = function(){
    var me = this;
    return new Promise(function (resolve, reject) {

        js.register(addressSchema);

        var isValid = js.validate(me.player, playerSchema);

        if (isValid.length == 0) {
            resolve();
        } else {
            reject({ reason: "Player is not compatible to schema", message: "Player is not compatible to schema", validationResult: isValid });
        }
    });
};

module.exports = Validator;

var playerSchema = {
    "id": "playerSchema", //important thing not to forget
    "$schema": "http://json-schema.org/draft-04/schema#",
    "title": "Player Schema",
    "type": "object",
    "properties": {
        "_id": {
            "type": ["string", "null", "object"]
        },
        "firstname": {
            "type": "string"
        },
        "lastname": {
            "type": "string"
        },
        "address": {
            "$ref": "addressSchema"
        },
        "homeClub": {
            "title": "Home Club Schema",
            "type": "object",
            "properties": {
                "name": {
                    "type": ["string", "null"]
                },
                "_id": {
                    "type": "string"
                }
            },
            "required": ["_id"]
        },
        "hcp": {
            "type": ["number", "integer"],
            "minimum": 0,
            "maximum": 54
        },
        "overrideHcp": {
            "type": ["null","boolean"]
        },
        "avatar": {
            "type": ["null", "string"]
        }
    },
    "required": ["firstname", "lastname", "homeClub", "hcp"]
}


module.exports = playerSchema;

var playerSchema = {
    "title": "Player Schema",
    "type": "object",
    "properties": {
        "_id": {
            "type": ["string", "null"]
        },
        "firstname": {
            "type": "string"
        },
        "lastname": {
            "type": "string"
        },
        "address": {
            "title": "Address Schema",
            "type": ["object", "null"],
            "properties": {
                "street": {
                    "type": "string"
                },
                "houseNo": {
                    "type": "string"
                },
                "zip": {
                    "type": "string"
                },
                "city": {
                    "type": "string"
                },
                "country": {
                    "type": "string"
                }
            }
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
            "minimum": -54
        }
    },
    "required": ["firstname", "lastname", "homeClub", "hcp"]
}


module.exports = playerSchema;
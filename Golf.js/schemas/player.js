
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
                    "type": ["string", "null"]
                },
                "houseNo": {
                    "type": ["string", "null"]
                },
                "zip": {
                    "type": ["string", "null"]
                },
                "city": {
                    "type": ["string", "null"]
                },
                "country": {
                    "type": ["string", "null"]
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
            "minimum": 0,
            "maximum": 54
        }
    },
    "required": ["firstname", "lastname", "homeClub", "hcp"]
}


module.exports = playerSchema;
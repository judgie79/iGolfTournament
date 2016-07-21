
var clubSchema = {
    "title": "Club Schema",
    "type": "object",
    "properties": {
        "_id": {
            "type": ["string", "null"]
        },
        "name": {
            "type": "string"
        },
        "address": {
            "title": "Address Schema",
            "type": ["object", "null"],
            "properties": {
                "street": {
                    "type": ["string", "null"],
                },
                "houseNo": {
                    "type": ["string", "null"],
                },
                "zip": {
                    "type": ["string", "null"],
                },
                "city": {
                    "type": ["string", "null"],
                },
                "country": {
                    "type": ["string", "null"],
                    "maxLength": 2,
                    "minLength": 2
                }
            }
        }
    },
    "required": ["name"]
}


module.exports = clubSchema;
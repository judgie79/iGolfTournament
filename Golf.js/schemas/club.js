
var clubSchema = {
    "title": "Club Schema",
    "type": "object",
    "properties": {
        "_id": {
            "type": "string"
        },
        "name": {
            "type": "string"
        },
        "address": {
            "title": "Address Schema",
            "type": "object",
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
                    "type": "string",
                    "maxLength": 2,
                    "minLength": 2
                }
            }
        }
    },
    "required": ["name"]
}


module.exports = clubSchema;
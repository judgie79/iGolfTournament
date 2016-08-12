
var clubSchema = {
    "id": "clubSchema", //important thing not to forget
    "$schema": "http://json-schema.org/draft-04/schema#",
    "title": "Club Schema",
    "type": "object",
    "properties": {
        "_id": {
            "type": ["string", "null", "object"]
        },
        "name": {
            "type": "string"
        },
        "address": {
            "$ref": "addressSchema"
        },
        "localRules": {
            "type": ["null", "string"]
        }
    },
    "required": ["name"]
}


module.exports = clubSchema;
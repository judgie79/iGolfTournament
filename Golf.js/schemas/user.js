
var userSchema = {
    "id": "userSchema", //important thing not to forget
    "$schema": "http://json-schema.org/draft-04/schema#",
    "title": "User Schema",
    "type": "object",
    "properties": {
        "_id": {
            "type": ["string", "null", "object"]
        },
        "username": {
            "type": "string"
        },
        "password": {
            "type": "string"
        },
        "emai": {
            "type": "string"
        }
    }
}


module.exports = userSchema;
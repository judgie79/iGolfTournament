var addressSchema = {
    "id": "addressSchema", //important thing not to forget
    "$schema": "http://json-schema.org/draft-04/schema#",
    "title": "Address Schema",
    "type": "object",
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


module.exports = addressSchema;
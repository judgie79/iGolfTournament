var holeSchema = {
    "id": "holeSchema", //important thing not to forget
    "$schema": "http://json-schema.org/draft-04/schema#",
    "title": "Hole Schema",
    "type": "object",
    "properties": {
        "holeId": {
            "type": ["string", "object"]
        },
        "number": {
            "type": "number",
            "minimum": 1
        },
        "distance": {
            "type": "number",
            "minimum": 1
        },
        "par": {
            "type": "number",
            "minimum": 1,
            "maximum": 6
        },
        "hcp": {
            "type": "number",
            "minimum": 1
        },
        "courseImage": {
            "type" : ["null", "string"]
        }
    },
    "required": ["number", "distance", "par", "hcp"]
}


module.exports = holeSchema;
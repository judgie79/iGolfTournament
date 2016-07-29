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
            "minimum": 3,
            "maximum": 5
        },
        "hcp": {
            "type": "number",
            "minimum": 1
        }
    },
    "required": ["number", "distance", "par", "hcp"]
}


module.exports = holeSchema;
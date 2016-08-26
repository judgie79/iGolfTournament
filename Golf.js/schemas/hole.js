var holeSchema = {
    "id": "holeSchema", //important thing not to forget
    "$schema": "http://json-schema.org/draft-04/schema#",
    "title": "Hole Schema",
    "type": "object",
    "properties": {
        "_id": {
            "type": ["string", "object"]
        },
        "clubId": {
            "type": ["string", "object"]
        },
        "name": {
            "type": ["string", "null"]
        },
        "courseImage": {
            "type" : ["null", "string"]
        }
    },
    "required": ["number", "distance", "par", "hcp"]
}


module.exports = holeSchema;
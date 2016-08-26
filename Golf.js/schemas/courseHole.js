var courseHoleSchema = {
    "id": "courseHoleSchema", //important thing not to forget
    "$schema": "http://json-schema.org/draft-04/schema#",
    "title": "CourseHole Schema",
    "type": "object",
    "allOf": [ { "type": "holeSchema" } ],
    "properties": {
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
        }
    },
    "required": ["number", "distance", "par", "hcp"]
}


module.exports = courseHoleSchema;
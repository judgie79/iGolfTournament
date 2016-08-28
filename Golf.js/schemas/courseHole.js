var courseHoleSchema = {
    "id": "courseHoleSchema", //important thing not to forget
    "$schema": "http://json-schema.org/draft-04/schema#",
    "title": "CourseHole Schema",
    "type": "object",
    "allOf": [
        // Here, we include our "core" address schema...
        { "$ref": "holeSchema" },

        // ...and then extend it with stuff specific to a shipping
        // address
        {
            "properties": {
                "courseId": {
                    "type": ["string", "object"]
                },
                "teeboxId": {
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
                "frontOrBack": {
                    "enum": ["front", "back"]
                }
            },
            "required": ["number", "distance", "par", "hcp", "frontOrBack"]
        }
    ]
}


module.exports = courseHoleSchema;
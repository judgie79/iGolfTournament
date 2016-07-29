var teeboxSchema = { 
    "id": "teeboxSchema", //important thing not to forget
    "$schema": "http://json-schema.org/draft-04/schema#",
    "title": "Teebox Schema",
    "type": "object",
        "properties": {
            "color": {
                "type": "string",
                "pattern": "^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$",
                "format": "color"
            },
            "name": {
                "type": "string"
            },
            "distance": {
                "type": "number",
                "minimum": 1
            },
            "par": {
                "type": "number",
                "minimum": 1
            },
            "courseRating": {
                "type": "number",
                "minimum": 1
            },
            "slopeRating": {
                "type": "number",
                "minimum": 1
            },
            "holes": {
                "type": ["object", "null"],
                "properties" : {
                    "front" : {
                        "type": ["array", "null"],
                        "items": {
                            "$ref" : "holeSchema"
                        }
                    },
                    "back" : {
                        "type": ["array", "null"],
                        "items": {
                            "$ref" : "holeSchema"
                        }
                    }
                }
            },
            "required": ["color", "name", "distance", "par", "courseRating", "slopeRating"]
        }
};

module.exports = teeboxSchema;
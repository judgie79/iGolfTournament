var courseScheme = {
    "id": "courseSchema", //important thing not to forget
    "$schema": "http://json-schema.org/draft-04/schema#",
    "title": "Course Schema",
    "type": "object",
    "properties": {
        "_id": {
            "type": ["string", "null", "object"]
        },
        "clubId": {
            "type": ["string", "object"]
        },
        "name": {
            "type": "string"
        },
        "teeboxes": {
            "type": "array",
            "items": {
                "type": "object",
                "properties": {
                    "_id": {
                        "type": ["string", "object", "null"]
                    },
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
                        "minimum": 0
                    },
                    "par": {
                        "type": "number",
                        "minimum": 0
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
            }
        }
    },
    "required": ["clubId", "name"]
}

module.exports = courseScheme;
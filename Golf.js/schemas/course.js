﻿var courseScheme = {
    "id": "courseSchema", //important thing not to forget
    "$schema": "http://json-schema.org/draft-04/schema#",
    "title": "Course Schema",
    "type": "object",
    "properties": {
        "_id": {
            "type": ["string", "null"]
        },
        "clubId": {
            "type": "string"
        },
        "name": {
            "type": "string"
        },
        "teeboxes": {
            "type": "array",
            "items": {
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
                        "type": ["array", "null"],
                        "items": {
                            "type": "object",
                            "properties": {
                                "holeId": {
                                    "type": "string"
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
                    },
                    "required": ["color", "name", "distance", "par", "courseRating", "slopeRating"]
                }
            }
        }
    },
    "required": ["clubId", "name"]
}

module.exports = courseScheme;
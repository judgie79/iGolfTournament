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
            "type": ["array", "null"],
            "items": {
                "$ref": "teeboxSchema"
            }
        }
    },
    "required": ["clubId", "name"]
}

module.exports = courseScheme;
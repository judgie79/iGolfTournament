var courseScheme = {
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
                "$ref": "teeboxSchema"
            }
        }
    },
    "required": ["clubId", "name"]
}

module.exports = courseScheme;
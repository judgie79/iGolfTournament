﻿var HoleCollectionController = function (teeName, holeGroup, teeIndex, holeCount, holeOffset) {

    this.teeName = teeName;
    this.teeIndex = teeIndex;
    this.holeCount = holeCount;
    this.holeGroup = holeGroup;
    this.holeOffset = holeOffset;
};

HoleCollectionController.prototype.addHole = function () {
    $("#scoreSheetEditorTableColumns_" + this.teeName).append("<col width=\"60\" />");
    $("#scoreSheetEditorTableRowNumber_" + this.teeName).append($("<th><input type=\"hidden\" value=\"\" name=\"Course.TeeBoxes[" + this.teeIndex + "].Holes." + this.holeGroup + "[" + this.holeCount + "].HoleId\"><input type=\"text\" class=\"form-control col-sm-3\" value=\"" + (this.holeCount + this.holeOffset + 1) + "\" name=\"Course.TeeBoxes[" + this.teeIndex + "].Holes." + this.holeGroup + "[" + this.holeCount + "].Number\"></th>"))
    $("#scoreSheetEditorTableRowPar_" + this.teeName).append($("<td><input class=\"form-control col-sm-3\" type=\"text\" value=\"\" name=\"Course.TeeBoxes[" + this.teeIndex + "].Holes." + this.holeGroup + "[" + this.holeCount + "].Par\"></td>"))
    $("#scoreSheetEditorTableRowHcp_" + this.teeName).append($("<td><input class=\"form-control col-sm-3\" type=\"text\" value=\"\" name=\"Course.TeeBoxes[" + this.teeIndex + "].Holes." + this.holeGroup + "[" + this.holeCount + "].Hcp\"></td>"))
    $("#scoreSheetEditorTableRowDistance_" + this.teeName).append($("<td><input class=\"form-control col-sm-3\" type=\"text\" value=\"\" name=\"Course.TeeBoxes[" + this.teeIndex + "].Holes." + this.holeGroup + "[" + this.holeCount + "].Distance\"></td>"))
    this.holeCount++;
};

HoleCollectionController.prototype.removeLastHole = function () {
    $("#scoreSheetEditorTableColumns_" + this.teeName).children().last().remove();
    $("#scoreSheetEditorTableRowNumber_" + this.teeName).children().last().remove();
    $("#scoreSheetEditorTableRowPar_" + this.teeName).children().last().remove();
    $("#scoreSheetEditorTableRowHcp_" + this.teeName).children().last().remove();
    $("#scoreSheetEditorTableRowDistance_" + this.teeName).children().last().remove();
    this.holeCount--;
}


////////////////////

var ParticipantCollectionController = function (participantCount) {

    this.participantCount = participantCount;
};

HoleCollectionController.prototype.addParticipant = function () {
    $("#tableEditParticipants").append("<tr><td></td><td></td><td></td><td></td><td></td></tr>");
    this.participantCount++;
};
$(function () {
    var editor = new wysihtml.Editor(document.getElementById('{0}'), {
        toolbar:document.getElementById('{1}'),
        parserRules:  wysihtmlParserRules
    });

    var log = document.getElementById('{2}');


    editor
        .on('load', function() {
                    log.innerHTML += '<div>load</div>';
                })
        .on('focus', function() {
                    log.innerHTML += '<div>focus</div>';
                })
        .on('blur', function() {
                    log.innerHTML += '<div>blur</div>';
                })
        .on('change', function() {
                    log.innerHTML += '<div>change</div>';
                })
        .on('paste', function() {
                    log.innerHTML += '<div>paste</div>';
                })
        .on('newword:composer', function() {
                    log.innerHTML += '<div>newword:composer</div>';
                })
        .on('undo:composer', function() {
                    log.innerHTML += '<div>undo:composer</div>';
                })
        .on('redo:composer', function() {
                    log.innerHTML += '<div>redo:composer</div>';
                });
});
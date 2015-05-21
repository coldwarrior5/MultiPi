$(document).ready(function () {

    var questionNumber = 0;
    var questionBank = new Array();
    var correct = new Array();
    var stage = "#game1";
    var stage2 = new Object;
    var questionLock = false;
    var score = 0;
    var logged = $('#logged').val();
    var exam = true;
    var area = true;
    var priorityNew = true;
    var noClass = 1;
    var numberOfQuestions = 0;
    var noArea = "";
    var subject = "";
    var userChoice = new Array();
    var choiceNumber = 0;
    var minNumQuestions = 10;
    var maxNumQuestions = 50;
    var chosenNumQuestions = minNumQuestions;

    /*
    if (logged) {
        choiceNumber = 0
        displayOptions("Novi ispit", "Stari ispiti");
    }
    else {
        choiceNumber = 1;
        displayOptions("Kartice za učenje", "Ispit");
    }*/
    $.getJSON('../../Scripts/activity.json', function (data) {
        answered = data.answered;
        for (i = 0; i < data.quizlist.length; i++) {
            questionBank[i] = new Array;
            questionBank[i][0] = data.quizlist[i].question;
            questionBank[i][1] = data.quizlist[i].option1;
            questionBank[i][2] = data.quizlist[i].option2;
            questionBank[i][3] = data.quizlist[i].option3;
			correct[i] = parseInt(data.quizlist[i].trueAnswer);
        }
        numberOfQuestions = questionBank.length;
        var rnd = Math.random() * numberOfQuestions;
        rnd = Math.ceil(rnd)-1;
        var rnd3;
        if (rnd < 2) {
            rnd += 2;
        }
        var rnd4 = Math.random() * (numberOfQuestions - rnd);
        rnd4 = Math.ceil(rnd4) - 1;
        rnd3 = numberOfQuestions - rnd - rnd4;
        numberOfQuestions = rnd;
        questionNumber = rnd3;
        displayQuestion(numberOfQuestions);
        })
      

    function displayQuestion(numberOfQuestions) {
        var correctAnswer = correct[questionNumber];
        $(stage).append('<div class="questionText">' + questionBank[questionNumber][0]);
        var questionArray = new Array(numberOfQuestions);
        for (var iterate = 1; iterate <= numberOfQuestions; iterate++) {
            while (true) {
                var rnd = Math.random() * numberOfQuestions;
                rnd = Math.ceil(rnd);
                if (typeof questionArray[rnd] === 'undefined') {
                    if (correctAnswer === iterate) {
                        correct[questionNumber] = rnd;
                    }
                    questionArray[rnd] = questionBank[questionNumber][iterate];
                    break;
                }
            }
        }
        for (var iterate = 1; iterate <= numberOfQuestions; iterate++) {
            $(stage).append('</div><div id="' + iterate + '" class="option">' + questionArray[iterate]);
        }


        $('.option').click(function () {
            if (questionLock == false) {
                questionLock = true;
                //correct answer
                if (parseInt(this.id) == correct[questionNumber]) {
                    $(stage).append('<div class="feedback1">Točno</div>');
                    score++;
                }
                //wrong answer	
                if (parseInt(this.id) != correct[questionNumber]) {
                    $(stage).append('<div class="feedback2">Krivo</div>');
                }
                setTimeout(function () { changeQuestion() }, 1000);
            }
        })
    }//display question

    function scrollOptions(choice1, choice2) {
        if (window.XMLHttpRequest) {// code for IE7+, Firefox, Chrome, Opera, Safari
            xmlhttp = new XMLHttpRequest();
        }
        else {// code for IE6, IE5
            xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
        }
        
        if (choice2 != "") {
            $(stage).append('<div class="scrollText"><span>' + choice1 + '</span><div> @using (Ajax.BeginForm("ChooseSubject", "Home", new AjaxOptions { HttpMethod = "post"})){');
            $(stage).append('<select name="Area" onChange="chosenArea(this.value)">');
            $(stage).append('</select><select name ="Subject" onChange=chosenSubject(this.value)>');
            $(stage).append('</select><select name ="Number" onChange=chosenNumber(this.value)>');
            $(stage).append('</select><div id="true" class="begin">Započni</div>}</div></div><div class="scrollText"><span>' + choice2 + '</span><div>@using (Ajax.BeginForm("ChooseClass", "Home", new AjaxOptions { HttpMethod = "post"})){');
            $(stage).append('<select name="Class" onChange="chosenClass(this.value)">');
            $(stage).append('</select><select name ="Number" onChange=chosenNumber(this.value)>');
            $(stage).append('</select><div id="false" class="begin">Započni</div></div>}</div>');
        }
        else {
            $(stage).append('<div class="scrollTextSingle"><span>' + choice1 + '</span><div><form>');
            $(stage).append('<select name="Test" onChange="chosenTest(this.value)">');
            $(stage).append('</select><div id="false" class="begin">Započni</div></form></div></div>');
        }

        $('.optionText').click(function () {
            if (questionLock == false) {
                questionLock = true;
                var result = false;
                var choice1;
                var choice2;
                var final = false;
                //correct answer
                if (this.id == "true") {
                    result = true;
                }

                switch (choiceNumber) {
                    case 0:
                        priorityNew = result;
                        if (result) {
                            choice1 = "Kartice za učenje";
                            choiceNumber = 1;
                            choice2 = "Ispit";
                        }
                        else {
                            choice1 = "Pregled"
                        }
                        break;
                    case 1:
                        exam = !result;
                        choice1 = "Područje";
                        choice2 = "Razred";
                        final = true
                        break;
                }
                setTimeout(function () {
                    if (priorityNew) {

                        if (final) {
                            scrollOptionBar(choice1, choice2);
                        }
                        else {
                            changeOptionBar(choice1, choice2)
                        }
                    }
                    else {
                        scrollOptionBar(choice1)
                    }

                }, 1000);
            }
        })
    }//display question

    function displayOptions(choice1, choice2) {
        $(stage).append('<div id="true" class="optionText"><span>' + choice1);
        $(stage).append('</span></div><div id="false" class="optionText"><span>' + choice2 + '</span></div>');
        
        $('.optionText').click(function () {
            if (questionLock == false) {
                questionLock = true;
                var result = false;
                var choice1;
                var choice2;
                var final=false;
                //correct answer
                if (this.id=="true") {
                    result = true;
                }
               
                switch (choiceNumber) {
                    case 0:
                        priorityNew = result;
                        if (result) {
                            choice1 = "Kartice za učenje";
                            choiceNumber = 1;
                            choice2 = "Ispit";
                        }
                        else {
                            choice1 = "Pregled";
                            choice2 = "";
                        }
                        break;
                    case 1:
                        exam = !result;
                        choice1 = "Područje";
                        choice2 = "Razred";
                        final = true;
                        break;
                }
                setTimeout(function () {
                    if (priorityNew) {
                        
                        if (final) {
                            scrollOptionBar(choice1,choice2);
                        }
                        else {
                            changeOptionBar(choice1, choice2);
                        }
                    }
                    else {
                        scrollOptionBar(choice1, choice2);
                    }
                    
                }, 1000);
            }
        })
    }//display question


    function changeOptionBar(choice1, choice2) {
        if (stage == "#game1") { stage2 = "#game1"; stage = "#game2";}
        else { stage2 = "#game2"; stage = "#game1"; }
        displayOptions(choice1, choice2);
        $(stage2).animate({ "right": "+=800px" }, "slow", function () { $(stage2).css('right', '-800px'); $(stage2).empty(); });
        $(stage).animate({ "right": "+=800px" }, "slow", function () { questionLock = false; });
    }//change options

    function scrollOptionBar(choice1, choice2) {
        if (stage == "#game1") { stage2 = "#game1"; stage = "#game2"; }
        else { stage2 = "#game2"; stage = "#game1"; }
        scrollOptions(choice1, choice2);
        $(stage2).animate({ "right": "+=800px" }, "slow", function () { $(stage2).css('right', '-800px'); $(stage2).empty(); });
        $(stage).animate({ "right": "+=800px" }, "slow", function () { questionLock = false; });
    }//change options


    function changeQuestion() {

        questionNumber++;

        if (stage == "#game1") { stage2 = "#game1"; stage = "#game2"; }
        else { stage2 = "#game2"; stage = "#game1"; }

        if (questionNumber < questionNumber+numberOfQuestions) { displayQuestion(numberOfQuestions); } else { displayFinalSlide(); }

        $(stage2).animate({ "right": "+=800px" }, "slow", function () { $(stage2).css('right', '-800px'); $(stage2).empty(); });
        $(stage).animate({ "right": "+=800px" }, "slow", function () { questionLock = false; });
    }//change question

    function displayFinalSlide() {

        $(stage).append('<div class="questionText">Završili ste kviz!<br><br>Ukupno pitanja: ' + numberOfQuestions + '<br>Točni odgovori: ' + score + '</div>');

    }//display final slide


});//doc ready
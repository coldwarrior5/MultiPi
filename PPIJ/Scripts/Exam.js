var idArea = 0;
var idSubject = 0;
$(document).ready(function () {

    var questionNumber = -1;
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

    var userChoice = new Array();
    var choiceNumber = 0;
    var chosenNumQuestions = 0;
    var minNumQuestions = 10;
    var maxNumQuestions = 30;
    
    if (logged=="true") {
        choiceNumber = 0
        displayOptions("Novi ispit", "Stari ispiti");
    }
    else {
        choiceNumber = 1;
        displayOptions("Kartice za učenje", "Ispit");
    }/*
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

        displayQuestion(3);
        })
      
      */
    function displayQuestion() {
        var correctAnswers = new Array();
        var questionPicture = new Array(questionBank[questionNumber][5].length);
        for(var i = 0; i<questionBank[questionNumber][5].length; i++)
        {
            if(questionBank[questionNumber][5][i][3]=="true")
            {
                correctAnswers.push(questionBank[questionNumber][5][i][3]);
            }
        }
        $(stage).append('<div class="questionText">' + questionBank[questionNumber][1] + '<br/>' + questionBank[questionNumber][3]);
        if (questionBank[questionNumber][2] != "null")
        {
            $(stage).append('<br/><img src="../../Images/Exam/' + questionBank[questionNumber][2]+'.jpg"/>');
        }
        var questionArray = new Array(questionBank[questionNumber][5].length);
        for (var iterate = 1; iterate <= questionBank[questionNumber][5].length; iterate++) {
            while (true) {
                var rnd = Math.random() * questionBank[questionNumber][5].length;
                rnd = Math.ceil(rnd);
                if (typeof questionArray[rnd] === 'undefined') {
                    if (correctAnswers === iterate) {
                        correct[questionNumber] = rnd;
                    }
                    questionArray[rnd] = questionBank[questionNumber][5][iterate][1];
                    if (questionBank[questionNumber][5][iterate][2] != "null")
                    {
                        questionPicture[rnd] = questionBank[questionNumber][5][iterate][2]
                    }
                    else {
                        questionPicture[rnd] = "";
                    }
                    break;
                }
            }
        }
        for (var iterate = 1; iterate <= questionBank[questionNumber][5].length; iterate++) {
            $(stage).append('</div><div id="' + iterate + '" class="option">' + questionArray[iterate] + '<img src="../../Images/Exam/' + questionPicture[iterate]+'.jpg"/>');
        }


        $('.option').click(function () {
            if (questionLock == false) {
                questionLock = true;
                //correct answer
                if (parseInt(this.id) == correct[questionNumber]) {
                    $(stage).append('<div class="feedback1">Tocno</div>');
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
       
        if (choice2 != "") {
            $(stage).load("/Home/ExamPartial", function (responseText, textStatus, XMLHttpRequest) {
              
                $('.begin').click(function (e) {
                    e.preventDefault();
                    if (questionLock == false) {
                        questionLock = true;
                        $.getJSON('http://localhost/Home/LoadQuestionsSubject?idSubject='+idSubject, function (data) {
                            for (i = 0; i < data.quizlist.length; i++) {
                                questionBank[i] = new Array;
                                questionBank[i][0] = data.quizlist[i].idQuestion;
                                questionBank[i][1] = data.quizlist[i].question;
                                questionBank[i][2] = data.quizlist[i].picture;
                                questionBank[i][3] = data.quizlist[i].idInstruction;
                                questionBank[i][4] = data.quizlist[i].singleChoice;
                                questionBank[i][5] = new Array();
                                for (j = 0; j < data.quizlist[i].answers.length; j++) {
                                    questionBank[i][5][j] = new Array();
                                    questionBank[i][5][j][0] = data.quizlist[i].answers[j].idAnswer;
                                    questionBank[i][5][j][1] = data.quizlist[i].answers[j].answer;
                                    questionBank[i][5][j][2] = data.quizlist[i].answers[j].picture;
                                    questionBank[i][5][j][3] = data.quizlist[i].answers[j].correct;
                                }
                            }
                            numberOfQuestions = questionBank.length;
                        });
                        
                        setTimeout(function () {
                            changeQuestion();
                        }, 3000);
                    }
                });
            });
        }
        else {
            $(stage).append('<div class="scrollTextSingle"><span>' + choice1 + '</span><div><form>');
            $(stage).append('<select name="Test" onChange="chosenTest(this.value)">');
            $(stage).append('</select><div id="false" class="begin">Započni</div></form></div></div>');
        }


        
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

        if (questionNumber < numberOfQuestions) { displayQuestion(); } else { displayFinalSlide(); }

        $(stage2).animate({ "right": "+=800px" }, "slow", function () { $(stage2).css('right', '-800px'); $(stage2).empty(); });
        $(stage).animate({ "right": "+=800px" }, "slow", function () { questionLock = false; });
    }//change question

    function displayFinalSlide() {

        $(stage).append('<div class="questionText">Zavrsili ste kviz!<br><br>Ukupno pitanja: ' + numberOfQuestions + '<br>Tocnih odgovora: ' + score + '</div>');

    }//display final slide

});//doc ready
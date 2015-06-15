var idArea = 0;
var noClass = 1;
var idSubject = 0;
var questionNumber = -1;
var correctAnswer = [];
var chosenNumQuestions = 0;
var minNumQuestions = 10;
var maxNumQuestions = 30;
var RandomizeArray = [];
$(document).ready(function () {
    var questionBank = new Array();
    var correct = new Array();
    var stage = "#game1";
    var stage2 = new Object;
    var questionLock = false;
    var score = 0;
    var answered = 0;
    var logged = $('#logged').val();
    var exam = true;
    var area = true;
    var priorityNew = true; 
    var numberOfQuestions = 0;
    var userChoice = new Array();
    var choiceNumber = 0;

    if (logged=="true") {
        choiceNumber = 0
        displayOptions("Novi ispit", "Stari ispiti");
    }
    else {
        choiceNumber = 1;
        displayOptions("Kartice za učenje", "Ispit");
    }
    function displayQuestion() {
        correctAnswer.length = 0;
        answered = 0;
        var toBeAppended = ""
        var questionPicture = new Array(questionBank[currentQuestion][5].length);
        for(var i = 0; i<questionBank[currentQuestion][5].length; i++)
        {
            if(questionBank[currentQuestion][5][i][3]=="True")
            {
                correctAnswer.push(i);
            }
        }
        toBeAppended = toBeAppended.concat('<div class="questionText"><h3 style="margin-top:20px">' + questionBank[currentQuestion][1] + '</h3><h4  style="margin-top:10px">' + questionBank[currentQuestion][3] + '</h4>');
        if (questionBank[currentQuestion][2] != "null")
        {
            toBeAppended = toBeAppended.concat('<img style="margin-top:10px"src="../../Images/Exam/' + questionBank[currentQuestion][2] + '.jpg"/>');
        }
        toBeAppended = toBeAppended.concat('</div><div class="answer">');
        correct[currentQuestion] = new Array();
        var questionArray = new Array(questionBank[currentQuestion][5].length);
        for (var iterate = 0; iterate < questionBank[currentQuestion][5].length; iterate++) {
            while (true) {
                var rnd = Math.random() * questionBank[currentQuestion][5].length;
                rnd = Math.ceil(rnd)-1;
                if (typeof questionArray[rnd] === 'undefined') {
                    if (correctAnswer.lastIndexOf(iterate)!=-1) {
                        correct[currentQuestion].push(rnd);
                    }
                    questionArray[rnd] = questionBank[currentQuestion][5][iterate][1];
                    if (questionBank[currentQuestion][5][iterate][2] != "null")
                    {
                        questionPicture[rnd] = questionBank[currentQuestion][5][iterate][2]
                    }
                    else {
                        questionPicture[rnd] = "";
                    }
                    break;
                }
            }
        }
        for (var iterate = 0; iterate < questionBank[currentQuestion][5].length; iterate++) {
            if (questionArray[iterate] == "") {
                toBeAppended = toBeAppended.concat('<div id="' + iterate + '"class="option' + questionBank[currentQuestion][5].length + '"><img  src="../../Images/Exam/' + questionPicture[iterate] + '.jpg"/></div>');
            }
            else {
                toBeAppended=toBeAppended.concat('<div id="' + iterate + '" class="option' + questionBank[currentQuestion][5].length + '"><span>' + questionArray[iterate] + '</span></div>');
            }
        }
        toBeAppended = toBeAppended.concat('</div>');
        $(stage).append(toBeAppended);
        /*
        $('.picture').click(function () {
            if (questionLock == false) {
                more = false;
                questionLock = true;
                //correct answer
                if (correct[currentQuestion].lastIndexOf(parseInt(this.id))!=-1) {
                    answered++;
                    if (correct[currentQuestion].length > 1 && answered < correct[currentQuestion].length) {
                        document.getElementById(parseInt(this.id)).style.pointerEvents = "none";
                        questionLock = false;
                        more = true;
                    }
                    else if (correct[currentQuestion].length > 1 && answered == correct[currentQuestion].length) {
                        score++;
                        $(stage).append('<div class="feedback1">Točno</div>');
                    }
                    else if(correct[currentQuestion].length == 1) {
                        score++;
                        $(stage).append('<div class="feedback1">Točno</div>');
                    }
                }
                //wrong answer	
                else{
                    $(stage).append('<div class="feedback2">Krivo</div>');
                }
                if (!more)
                {
                    setTimeout(function () { changeQuestion() }, 1000);
                }
            }
        })*/
        $('.option2').click(function () {
            if (questionLock == false) {
                more = false;
                questionLock = true;
                //correct answer
                if (correct[currentQuestion].lastIndexOf(parseInt(this.id)) != -1) {
                    document.getElementById(parseInt(this.id)).style.backgroundColor = "green";
                    document.getElementById(parseInt(this.id)).innerHTML = document.getElementById(parseInt(this.id)).innerHTML + '<img style="width:32px;height:32px;margin-left:20px" src="../../Images/Exam/smile.png"/>';
                    answered++;
                    if (correct[currentQuestion].length > 1 && answered < correct[currentQuestion].length) {
                        document.getElementById(parseInt(this.id)).style.pointerEvents = "none";
                        questionLock = false;
                        more = true;
                    }
                    else if (correct[currentQuestion].length > 1 && answered == correct[currentQuestion].length) {
                        score++;
                    }
                    else if (correct[currentQuestion].length == 1) {
                        score++;
                    }
                }
                else {
                    document.getElementById(parseInt(this.id)).style.backgroundColor = "red";
                    document.getElementById(parseInt(this.id)).innerHTML = document.getElementById(parseInt(this.id)).innerHTML + '<img style="width:32px;height:32px;margin-left:20px" src="../../Images/Exam/sad.png"/>';
                }
                if (!more) {
                    setTimeout(function () { changeQuestion() }, 1000);
                }
            }
        })
        $('.option3').click(function () {
            if (questionLock == false) {
                more = false;
                questionLock = true;
                //correct answer
                if (correct[currentQuestion].lastIndexOf(parseInt(this.id)) != -1) {
                    document.getElementById(parseInt(this.id)).style.backgroundColor = "green";
                    document.getElementById(parseInt(this.id)).innerHTML = document.getElementById(parseInt(this.id)).innerHTML + '<img style="width:32px;height:32px;margin-left:20px" src="../../Images/Exam/smile.png"/>';
                    answered++;
                    if (correct[currentQuestion].length > 1 && answered < correct[currentQuestion].length) {
                        document.getElementById(parseInt(this.id)).style.pointerEvents = "none";
                        questionLock = false;
                        more = true;
                    }
                    else if (correct[currentQuestion].length > 1 && answered == correct[currentQuestion].length) {
                        score++;
                    }
                    else if (correct[currentQuestion].length == 1) {
                        score++;
                    }
                }
                else {
                    document.getElementById(parseInt(this.id)).style.backgroundColor = "red";
                    document.getElementById(parseInt(this.id)).innerHTML = document.getElementById(parseInt(this.id)).innerHTML + '<img style="width:32px;height:32px;margin-left:20px" src="../../Images/Exam/sad.png"/>';
                }
                if (!more) {
                    setTimeout(function () { changeQuestion() }, 1000);
                }
            }
        })
        $('.option4').click(function () {
            if (questionLock == false) {
                more = false;
                questionLock = true;
                //correct answer
                if (correct[currentQuestion].lastIndexOf(parseInt(this.id)) != -1) {
                    document.getElementById(parseInt(this.id)).style.backgroundColor = "green";
                    document.getElementById(parseInt(this.id)).innerHTML = document.getElementById(parseInt(this.id)).innerHTML + '<img style="width:32px;height:32px;margin-left:20px" src="../../Images/Exam/smile.png"/>';
                    answered++;
                    if (correct[currentQuestion].length > 1 && answered < correct[currentQuestion].length) {
                        document.getElementById(parseInt(this.id)).style.pointerEvents = "none";
                        questionLock = false;
                        more = true;
                    }
                    else if (correct[currentQuestion].length > 1 && answered == correct[currentQuestion].length) {
                        score++;
                    }
                    else if (correct[currentQuestion].length == 1) {
                        score++;
                    }
                }
                else {
                    document.getElementById(parseInt(this.id)).style.backgroundColor = "red";
                    document.getElementById(parseInt(this.id)).innerHTML = document.getElementById(parseInt(this.id)).innerHTML + '<img style="width:32px;height:32px;margin-left:20px" src="../../Images/Exam/sad.png"/>';
                }
                if (!more) {
                    setTimeout(function () { changeQuestion() }, 1000);
                }
            }
        })
        $('.option5').click(function () {
            if (questionLock == false) {
                more = false;
                questionLock = true;
                //correct answer
                if (correct[currentQuestion].lastIndexOf(parseInt(this.id)) != -1) {
                    document.getElementById(parseInt(this.id)).style.backgroundColor = "green";
                    document.getElementById(parseInt(this.id)).innerHTML = document.getElementById(parseInt(this.id)).innerHTML + '<img style="width:32px;height:32px;margin-left:20px" src="../../Images/Exam/smile.png"/>';
                    answered++;
                    if (correct[currentQuestion].length > 1 && answered < correct[currentQuestion].length) {
                        document.getElementById(parseInt(this.id)).style.pointerEvents = "none";
                        questionLock = false;
                        more = true;
                    }
                    else if (correct[currentQuestion].length > 1 && answered == correct[currentQuestion].length) {
                        score++;
                    }
                    else if (correct[currentQuestion].length == 1) {
                        score++;
                    }
                }
                else {
                    document.getElementById(parseInt(this.id)).style.backgroundColor = "red";
                    document.getElementById(parseInt(this.id)).innerHTML = document.getElementById(parseInt(this.id)).innerHTML + '<img style="width:32px;height:32px;margin-left:20px" src="../../Images/Exam/sad.png"/>';
                }
                if (!more) {
                    setTimeout(function () { changeQuestion() }, 1000);
                }
            }
        })
        $('.option6').click(function () {
            if (questionLock == false) {
                more = false;
                questionLock = true;
                //correct answer
                if (correct[currentQuestion].lastIndexOf(parseInt(this.id)) != -1) {
                    document.getElementById(parseInt(this.id)).style.backgroundColor = "green";
                    document.getElementById(parseInt(this.id)).innerHTML = document.getElementById(parseInt(this.id)).innerHTML + '<img style="width:32px;height:32px;margin-left:20px" src="../../Images/Exam/smile.png"/>';
                    answered++;
                    if (correct[currentQuestion].length > 1 && answered < correct[currentQuestion].length) {
                        document.getElementById(parseInt(this.id)).style.pointerEvents = "none";
                        questionLock = false;
                        more = true;
                    }
                    else if (correct[currentQuestion].length > 1 && answered == correct[currentQuestion].length) {
                        score++;
                    }
                    else if (correct[currentQuestion].length == 1) {
                        score++;
                    }
                }
                else {
                    document.getElementById(parseInt(this.id)).style.backgroundColor = "red";
                    document.getElementById(parseInt(this.id)).innerHTML = document.getElementById(parseInt(this.id)).innerHTML + '<img style="width:32px;height:32px;margin-left:20px" src="../../Images/Exam/sad.png"/>';
                }
                if (!more) {
                    setTimeout(function () { changeQuestion() }, 1000);
                }
            }
        })
    }//display question


    function scrollOptions(choice1, choice2) {
        questionNumber = -1;
        RandomizeArray.length = 0;
        minNumQuestions = 10;
        maxNumQuestions = 30;
        if (choice2 != "") {
            $(stage).load("/Home/ExamPartial", function (responseText, textStatus, XMLHttpRequest) {
                wait();
              
                $('.begin').click(function (e) {
                    e.preventDefault();
                    if (questionLock == false) {
                        questionLock = true;
                        var result = false;
                        //correct answer
                        if (this.id == "true") {
                            result = true;
                        }
                        var current = "game1";
                        if (stage == "#game2") {
                            current = "game2";                        }
                        document.getElementById(current).innerHTML = '<div style="width:inherit;height:inherit;display:flex;align-items:center;justify-content:center;"><img src="../../Images/loading.gif" /></div>';

                        if (result) {
                            $.ajax({
                                url: '../Home/LoadQuestionsSubject?idSubject=' + idSubject,
                                dataType: 'json',
                                async: false,
                                success: function (data) {
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
                                }
                            });
                            
                        }
                        else {
                            $.ajax({
                                url: '../Home/LoadQuestionsClass?idClass=' + noClass,
                                dataType: 'json',
                                async: false,
                                success: function (data) {
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
                                }
                            });
                           
                        }
                        
                        setTimeout(function () {
                            if (exam) {
                                changeQuestion();
                            }
                            else {
                                changeCard();
                            }
                            }, 1000);
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

    function displayCard() {
        var toBeAppended = ""
        var toBeAppended2 = '<div class="fin">';
        var questionPicture = new Array(questionBank[currentQuestion][5].length);
        for (var i = 0; i < questionBank[currentQuestion][5].length; i++) {
            if (questionBank[currentQuestion][5][i][3] == "True") {
                if (questionBank[currentQuestion][5][i][1] == "") {
                    toBeAppended2 = toBeAppended2.concat('<div style= "width:765px"class="option3"><img  src="../../Images/Exam/' + questionBank[currentQuestion][5][i][2] + '.jpg"/></div>');
                }
                else {
                    toBeAppended2 = toBeAppended2.concat('<div style= "width:765px" class="option3"><span>' + questionBank[currentQuestion][5][i][1] + '</span></div>');
                }
            }
        }
        toBeAppended2 = toBeAppended2.concat('</div>');

        toBeAppended = toBeAppended.concat('<div class="fin"><h3 style="margin-top:40px">' + questionBank[currentQuestion][1] + '</h3>');
        if (questionBank[currentQuestion][2] != "null") {
            toBeAppended = toBeAppended.concat('<img style="margin-top:30px"src="../../Images/Exam/' + questionBank[currentQuestion][2] + '.jpg"/>');
        }
        toBeAppended = toBeAppended.concat('</div>');
        
        $(stage).empty();
        $(stage).append(toBeAppended);
        wait();

        $('.fin').click(function () {
            if (questionLock == false) {
                questionLock = true;
                //correct answer
                $(stage).empty();
                $(stage).append(toBeAppended2);
                questionLock = false;
                $('.option3').click(function () {
                    if (questionLock == false) {
                        questionLock = true;
                        //correct answer
                        changeCard();

                    }
                })
            }
        })

        

    }

    function displayOptions(choice1, choice2) {
        $(stage).append('<div id="true" class="optionText"><span style="padding-bottom:50px;">' + choice1);
        $(stage).append('</span></div><div id="false" class="optionText"><span style="padding-bottom:50px;">' + choice2 + '</span></div>');
        
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
        $(stage2).empty();
        $(stage2).append('<div style="width:inherit;height:inherit;display:flex;align-items:center;justify-content:center;"><img src="../../Images/loading.gif" /></div>');

        scrollOptions(choice1, choice2);
       
    }//change options

    function changeCard() {
        questionNumber++;

        while (true) {
            if (questionNumber >= chosenNumQuestions) {
                break;
            }
            var rnd = Math.random() * questionBank.length;
            rnd = Math.ceil(rnd) - 1;
            if (RandomizeArray.lastIndexOf(rnd) === -1) {
                RandomizeArray.push(rnd);
                currentQuestion = rnd;
                break;
            }
            else {
                continue;
            }
        }

        if (stage == "#game1") { stage2 = "#game1"; stage = "#game2"; }
        else { stage2 = "#game2"; stage = "#game1"; }

        if (questionNumber < chosenNumQuestions) { displayCard(); } else {
            displayFinalCard();
            wait();
        }
        
    }

    function changeQuestion() {

        questionNumber++;
        
        while (true) {
            if (questionNumber >= chosenNumQuestions) {
                break;
            }
            var rnd = Math.random() * questionBank.length;
            rnd = Math.ceil(rnd)-1;
            if (RandomizeArray.lastIndexOf(rnd) === -1) {
                RandomizeArray.push(rnd);
                currentQuestion = rnd;
                break;
            }
            else
            {
                continue;
            }    
        }

        if (stage == "#game1") { stage2 = "#game1"; stage = "#game2"; }
        else { stage2 = "#game2"; stage = "#game1"; }

        if (questionNumber < chosenNumQuestions) { displayQuestion(); } else { displayFinalSlide(); }
        $(stage2).animate({ "right": "+=800px" }, "slow", function () { $(stage2).css('right', '-800px'); $(stage2).empty(); });
        $(stage).animate({ "right": "+=800px" }, "slow", function () { questionLock = false; });
    }//change question

    function wait() {
        $(stage2).animate({ "right": "+=800px" }, "slow", function () { $(stage2).css('right', '-800px'); $(stage2).empty(); });
        $(stage).animate({ "right": "+=800px" }, "slow", function () { questionLock = false; });

    }

    function displayFinalSlide() {

        $(stage).append('<div class="fin"><h3 style="margin-top:30px">Bravo završio si kviz!</h3><h4 style="margin-top:10px">Ukupno pitanja: ' + chosenNumQuestions + '</h4><h4 style="margin-top:20px">Točnih odgovora: ' + score + '</h4><div class="begin" style="margin-top:30px;pointer-events:auto;float:none;margin:auto;width:300px">Želiš li ponoviti ispit?</div></div>');

        $('.begin').click(function () {
            if (questionLock == false) {
                questionLock = true;
                
                questionNumber = -1;
                chosenNumQuestions = 0;
                minNumQuestions = 10;
                maxNumQuestions = 30;
                score = 0;
                answered = 0;
                numberOfQuestions = 0
                choiceNumber = 0;

                if (logged == "true") {
                    choiceNumber = 0
                    changeOptionBar("Novi ispit", "Stari ispiti");
                }
                else {
                    choiceNumber = 1;
                    changeOptionBar("Kartice za učenje", "Ispit");
                }
            }
        });

    }//display final slide

    function displayFinalCard() {

        $(stage).append('<div class="fin"><h3 style="margin-top:30px">Prošli ste kroz sve kartice!</h3><h4 style="margin-top:10px">Ukupno kartica: ' + chosenNumQuestions + '</h4><div class="begin" style="margin-top:30px;pointer-events:auto;float:none;margin:auto;width:300px">Želiš li početi iznova?</div></div>');

        $('.begin').click(function () {
            if (questionLock == false) {
                questionLock = true;

                questionNumber = -1;
                chosenNumQuestions = 0;
                minNumQuestions = 10;
                maxNumQuestions = 30;
                score = 0;
                answered = 0;
                numberOfQuestions = 0
                choiceNumber = 0;

                if (logged == "true") {
                    choiceNumber = 0
                    changeOptionBar("Novi ispit", "Stari ispiti");
                }
                else {
                    choiceNumber = 1;
                    changeOptionBar("Kartice za učenje", "Ispit");
                }
            }
        });

    }//display final slide

});//doc ready
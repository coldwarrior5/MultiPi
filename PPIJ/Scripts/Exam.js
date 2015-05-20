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

    /*
    if (logged) {
        changeOptionBar("Novi ispit", "Stari ispiti");
    }
    if (!priorityNew) {
        changeOptionBar("Pregled", "");
    }
    else {
        changeOptionBar("Kartice za učenje", "Ispit");
        changeOptionBar("Područje", "Razred");
        displayQuestion();
    }
    
    */
    $.getJSON('../../Scripts/activity.json', function (data) {

        for (i = 0; i < data.quizlist.length; i++) {
            questionBank[i] = new Array;
            questionBank[i][0] = data.quizlist[i].question;
            questionBank[i][1] = data.quizlist[i].option1;
            questionBank[i][2] = data.quizlist[i].option2;
            questionBank[i][3] = data.quizlist[i].option3;
			correct[i] = parseInt(data.quizlist[i].trueAnswer);
        }
        numberOfQuestions = questionBank.length;


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
                    $(stage).append('<div class="feedback1">CORRECT</div>');
                    score++;
                }
                //wrong answer	
                if (parseInt(this.id) != correct[questionNumber]) {
                    $(stage).append('<div class="feedback2">WRONG</div>');
                }
                setTimeout(function () { changeQuestion() }, 1000);
            }
        })
    }//display question

    function changeOptionBar(choice1, choice2) {
        if (stage == "#game1") { stage2 = "#game1"; stage = "#game2"; }
        else { stage2 = "#game2"; stage = "#game1"; }
        if (choice1 == "Područje" && choice2 == "Razred") {
            displayChoice();
        }
        else if (choice2 == "") {

        }
        else {

        }
    }

    function changeQuestion() {

        questionNumber++;

        if (stage == "#game1") { stage2 = "#game1"; stage = "#game2"; }
        else { stage2 = "#game2"; stage = "#game1"; }

        if (questionNumber < numberOfQuestions) { displayQuestion(numberOfQuestions); } else { displayFinalSlide(); }

        $(stage2).animate({ "right": "+=800px" }, "slow", function () { $(stage2).css('right', '-800px'); $(stage2).empty(); });
        $(stage).animate({ "right": "+=800px" }, "slow", function () { questionLock = false; });
    }//change question

    function displayFinalSlide() {

        $(stage).append('<div class="questionText">You have finished the quiz!<br><br>Total questions: ' + numberOfQuestions + '<br>Correct answers: ' + score + '</div>');

    }//display final slide


});//doc ready
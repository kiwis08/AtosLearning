// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function onSelectDropdown() {
    const examTitle = document.getElementById("exam-title");
    const examDescription = document.getElementById("exam-description");
    alert(examTitle.value)
    document.getElementById("hidden-exam-title").value = examTitle.value;
    document.getElementById("hidden-exam-description").value = examDescription.value;
    document.getElementById("subject-form").submit();
}

function addQuestion() {
    const questionsList = document.getElementById('questions-list');
    const questionCount = questionsList.children.length;

    const newQuestionLi = document.createElement('li');

    newQuestionLi.textContent = `Pregunta ${questionCount + 1}`;
    newQuestionLi.classList.add('btn');
    newQuestionLi.classList.add('list-group-item');
    newQuestionLi.classList.add('border-bottom');

    newQuestionLi.style.backgroundColor = 'transparent';


    questionsList.appendChild(newQuestionLi);
}

function submitExam() {
    document.getElementById('exam-form').submit();
}

$('#recipeCarousel').carousel({
    interval: 10000
})

$('.carousel .carousel-item').each(function () {
    var minPerSlide = 3;
    var next = $(this).next();
    if (!next.length) {
        next = $(this).siblings(':first');
    }
    next.children(':first-child').clone().appendTo($(this));

    for (var i = 0; i < minPerSlide; i++) {
        next = next.next();
        if (!next.length) {
            next = $(this).siblings(':first');
        }

        next.children(':first-child').clone().appendTo($(this));
    }
});

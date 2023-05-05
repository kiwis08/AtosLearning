// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function onSelectDropdown(subject) {
    document.getElementById("subject-dropdown").textContent = subject;
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
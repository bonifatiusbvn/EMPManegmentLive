// Toggle password visibility
Array.from(document.querySelectorAll("form .auth-pass-inputgroup")).forEach(function (group) {
    Array.from(group.querySelectorAll(".password-addon")).forEach(function (toggleButton) {
        toggleButton.addEventListener("click", function () {
            var passwordInput = group.querySelector(".password-input");
            passwordInput.type === "password" ? passwordInput.type = "text" : passwordInput.type = "password";
        });
    });
});

// Password match validation
var password = document.getElementById("password-input");
var confirmPassword = document.getElementById("confirm-password-input");

function validatePassword() {
    if (password.value !== confirmPassword.value) {
        confirmPassword.setCustomValidity("Passwords Don't Match");
    } else {
        confirmPassword.setCustomValidity("");
    }
}

password.onchange = validatePassword;
confirmPassword.onkeyup = validatePassword; // Ensures it validates on typing as well

// Password strength indicator
var passwordContain = document.getElementById("password-contain");
var letter = document.getElementById("pass-lower");
var capital = document.getElementById("pass-upper");
var number = document.getElementById("pass-number");
var length = document.getElementById("pass-length");

password.onfocus = function () {
    passwordContain.style.display = "block";
};

password.onblur = function () {
    passwordContain.style.display = "none";
};

password.onkeyup = function () {
    // Lowercase letters
    if (password.value.match(/[a-z]/)) {
        letter.classList.remove("invalid");
        letter.classList.add("valid");
    } else {
        letter.classList.remove("valid");
        letter.classList.add("invalid");
    }

    // Uppercase letters
    if (password.value.match(/[A-Z]/)) {
        capital.classList.remove("invalid");
        capital.classList.add("valid");
    } else {
        capital.classList.remove("valid");
        capital.classList.add("invalid");
    }

    // Numbers
    if (password.value.match(/[0-9]/)) {
        number.classList.remove("invalid");
        number.classList.add("valid");
    } else {
        number.classList.remove("valid");
        number.classList.add("invalid");
    }

    // Length
    if (password.value.length >= 8) {
        length.classList.remove("invalid");
        length.classList.add("valid");
    } else {
        length.classList.remove("valid");
        length.classList.add("invalid");
    }
};

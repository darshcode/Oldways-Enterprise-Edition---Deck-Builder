document.addEventListener("DOMContentLoaded", function () {

    const sliders = document.querySelectorAll("[data-slider]");

    sliders.forEach(function (slider) {

        const slides = slider.querySelectorAll(".slide");
        const dots = slider.querySelectorAll(".slider-dot");

        const previousButton = slider.querySelector("[data-prev]");
        const nextButton = slider.querySelector("[data-next]");

        if (slides.length === 0) {
            return;
        }

        let currentIndex = 0;

        function showSlide(index) {

            if (index < 0) {
                index = slides.length - 1;
            }

            if (index >= slides.length) {
                index = 0;
            }

            currentIndex = index;

            slides.forEach(function (slide, i) {
                slide.classList.toggle("active", i === currentIndex);
            });

            dots.forEach(function (dot, i) {
                dot.classList.toggle("active", i === currentIndex);
            });
        }

        if (previousButton) {
            previousButton.addEventListener("click", function () {
                showSlide(currentIndex - 1);
            });
        }

        if (nextButton) {
            nextButton.addEventListener("click", function () {
                showSlide(currentIndex + 1);
            });
        }

        dots.forEach(function (dot) {

            dot.addEventListener("click", function () {

                const index = parseInt(
                    dot.getAttribute("data-slide")
                );

                showSlide(index);
            });

        });

        showSlide(0);
    });

});
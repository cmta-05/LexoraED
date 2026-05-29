(function () {
  const form = document.getElementById('lexQuizForm');
  if (!form) return;

  const slides = form.querySelectorAll('.lex-quiz-slide');
  const dots = form.querySelectorAll('.lex-quiz-dot');
  const btnPrev = document.getElementById('quizPrev');
  const btnNext = document.getElementById('quizNext');
  const btnSubmit = document.getElementById('quizSubmit');
  let current = 0;

  function showSlide(index) {
    slides.forEach((s, i) => {
      s.style.display = i === index ? 'block' : 'none';
    });
    dots.forEach((d, i) => {
      d.classList.toggle('active', i === index);
      d.classList.toggle('done', i < index);
    });
    if (btnPrev) btnPrev.style.visibility = index === 0 ? 'hidden' : 'visible';
    if (btnNext) btnNext.style.display = index === slides.length - 1 ? 'none' : 'inline-flex';
    if (btnSubmit) btnSubmit.style.display = index === slides.length - 1 ? 'inline-flex' : 'none';
    current = index;
  }

  function highlightChoices(slide) {
    slide.querySelectorAll('.lex-choice').forEach(label => {
      const input = label.querySelector('input[type="radio"]');
      label.classList.toggle('selected', input && input.checked);
      if (input) {
        input.addEventListener('change', () => {
          slide.querySelectorAll('.lex-choice').forEach(l => l.classList.remove('selected'));
          label.classList.add('selected');
        });
      }
    });
  }

  slides.forEach(highlightChoices);
  showSlide(0);

  if (btnPrev) {
    btnPrev.addEventListener('click', () => {
      if (current > 0) showSlide(current - 1);
    });
  }

  function slideAnswered(slide) {
    const radio = slide.querySelector('input[type="radio"]:checked');
    if (radio) return true;
    const text = slide.querySelector('input[type="text"], input:not([type="hidden"]):not([type="radio"])');
    return text && text.value.trim().length > 0;
  }

  if (btnNext) {
    btnNext.addEventListener('click', () => {
      const slide = slides[current];
      if (!slideAnswered(slide)) {
        slide.classList.add('lex-shake');
        setTimeout(() => slide.classList.remove('lex-shake'), 400);
        return;
      }
      if (current < slides.length - 1) showSlide(current + 1);
    });
  }

  form.addEventListener('submit', function (e) {
    const firstEmpty = Array.from(slides).findIndex(s => !slideAnswered(s));
    if (firstEmpty >= 0) {
      e.preventDefault();
      alert('Please answer all ' + slides.length + ' questions before submitting.');
      showSlide(firstEmpty);
    }
  });
})();

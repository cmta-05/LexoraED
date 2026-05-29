(function () {
    const CACHE_KEY = 'lexoraed_offline_lessons';

    function cacheLesson(id, title, content) {
        try {
            const cache = JSON.parse(localStorage.getItem(CACHE_KEY) || '{}');
            cache[id] = { title, content, cachedAt: new Date().toISOString() };
            localStorage.setItem(CACHE_KEY, JSON.stringify(cache));
        } catch (e) { /* storage unavailable */ }
    }

    const lessonBody = document.querySelector('.lex-lesson-body');
    const moduleId = document.body.dataset.moduleId;
    if (lessonBody && moduleId) {
        cacheLesson(moduleId, document.title, lessonBody.innerHTML);
    }

    window.addEventListener('online', function () {
        document.querySelectorAll('.lex-offline-badge').forEach(el => el.textContent = 'Back online — syncing progress');
    });

    window.addEventListener('offline', function () {
        const badge = document.createElement('div');
        badge.className = 'lex-alert lex-alert-info lex-offline-badge';
        badge.textContent = 'You are offline. Cached lessons remain available.';
        document.querySelector('.lex-content')?.prepend(badge);
    });
})();

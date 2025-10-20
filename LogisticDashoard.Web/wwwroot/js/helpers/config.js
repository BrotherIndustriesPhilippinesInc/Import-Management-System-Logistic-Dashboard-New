// config.js
window.API_BASE_URL = (function () {
    const hostname = window.location.hostname;

    if (hostname === 'localhost' || hostname === '127.0.0.1') {
        return 'https://localhost:7019';
    } else if (hostname === 'apbiphbpswb01') {
        return 'http://apbiphbpswb01:1117';
    } else {
        console.warn('Unknown host. Defaulting to localhost.');
        return 'https://localhost:7019';
    }
})();

// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Dark Theme Toggle
document.addEventListener('DOMContentLoaded', function() {
    const themeToggle = document.getElementById('themeToggle');
    const navbar = document.querySelector('nav.navbar');
    
    // Initialize theme from localStorage or system preference
    function initializeTheme() {
        const savedTheme = localStorage.getItem('theme');
        
        if (savedTheme) {
            applyTheme(savedTheme);
        } else if (window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches) {
            applyTheme('dark');
        } else {
            applyTheme('light');
        }
    }
    
    // Apply theme to body and navbar
    function applyTheme(theme) {
        const body = document.body;
        
        if (theme === 'dark') {
            body.classList.add('dark-theme');
            if (navbar) {
                navbar.classList.remove('navbar-light', 'bg-white');
                navbar.classList.add('navbar-dark', 'bg-dark');
            }
            if (themeToggle) {
                themeToggle.innerHTML = '☀️';
                themeToggle.setAttribute('title', 'Switch to Light Theme');
            }
        } else {
            body.classList.remove('dark-theme');
            if (navbar) {
                navbar.classList.remove('navbar-dark', 'bg-dark');
                navbar.classList.add('navbar-light', 'bg-white');
            }
            if (themeToggle) {
                themeToggle.innerHTML = '🌙';
                themeToggle.setAttribute('title', 'Switch to Dark Theme');
            }
        }
        
        localStorage.setItem('theme', theme);
    }
    
    // Toggle theme on button click
    if (themeToggle) {
        themeToggle.addEventListener('click', function(e) {
            e.preventDefault();
            const currentTheme = localStorage.getItem('theme') || 'light';
            const newTheme = currentTheme === 'dark' ? 'light' : 'dark';
            applyTheme(newTheme);
        });
    }
    
    // Initialize theme on page load
    initializeTheme();
});

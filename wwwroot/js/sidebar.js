/* ============================================================
   FINANCEME — Sidebar Logic (sidebar.js)

   In the MVC version, the sidebar HTML is already rendered
   by the _Sidebar.cshtml Razor partial. This script ONLY
   handles interactive behaviors (collapse, mobile, active link).

   REQUIRES: utils.js must be loaded first!
   ============================================================ */


/**
 * Initialize the sidebar.
 * The sidebar markup is already in the DOM (rendered by _Sidebar.cshtml).
 * We just set up the interactive behaviors.
 *
 * @param {string} [activePage] - the data-page value to highlight
 */
function initSidebar(activePage) {
  // --- 1. Highlight the active page ---
  highlightActivePage(activePage);

  // --- 2. Set up collapse toggle ---
  setupSidebarToggle();

  // --- 3. Set up mobile sidebar ---
  setupMobileSidebar();

  // --- 4. Restore collapsed state from localStorage ---
  restoreSidebarState();
}


/**
 * Highlight the currently active nav link.
 * Matches against the data-page attribute on each link,
 * or auto-detects from the URL pathname.
 */
function highlightActivePage(activePage) {
  if (!activePage) {
    // Try to detect from the URL path segment
    const path = window.location.pathname;
    const segments = path.split('/').filter(Boolean);
    // MVC URLs look like /Accounts/Index or /Accounts/Dashboard
    // Use the last meaningful segment (action), or second-to-last (controller)
    activePage = segments[segments.length - 1]?.toLowerCase()
              || segments[segments.length - 2]?.toLowerCase()
              || 'dashboard';
  }

  const links = document.querySelectorAll('.sidebar__link');
  links.forEach(link => {
    link.classList.remove('active');
    const page = link.getAttribute('data-page');
    if (page && page.toLowerCase() === activePage.toLowerCase()) {
      link.classList.add('active');
    }
  });
}


/**
 * Set up the collapse/expand toggle button.
 * Saves state to localStorage so it persists across pages.
 */
function setupSidebarToggle() {
  const sidebar = document.getElementById('sidebar');
  const toggleBtn = document.getElementById('sidebar-toggle');

  if (!sidebar || !toggleBtn) return;

  toggleBtn.addEventListener('click', function () {
    sidebar.classList.toggle('collapsed');

    // Rotate the chevron icon when collapsed
    const icon = toggleBtn.querySelector('svg');
    if (icon) {
      icon.style.transform = sidebar.classList.contains('collapsed')
        ? 'rotate(180deg)'
        : 'rotate(0deg)';
    }

    // Remember the state
    localStorage.setItem('sidebarCollapsed', sidebar.classList.contains('collapsed'));
  });
}


/**
 * Restore sidebar collapsed state from localStorage.
 */
function restoreSidebarState() {
  const sidebar = document.getElementById('sidebar');
  const toggleBtn = document.getElementById('sidebar-toggle');

  if (!sidebar || !toggleBtn) return;

  const isCollapsed = localStorage.getItem('sidebarCollapsed') === 'true';
  if (isCollapsed) {
    sidebar.classList.add('collapsed');
    const icon = toggleBtn.querySelector('svg');
    if (icon) icon.style.transform = 'rotate(180deg)';
  }
}


/**
 * Set up mobile sidebar behavior.
 * On mobile, the sidebar slides in as an overlay.
 */
function setupMobileSidebar() {
  const sidebar = document.getElementById('sidebar');
  const overlay = document.getElementById('sidebar-overlay');
  const mobileMenuBtn = document.getElementById('mobile-menu-btn');

  if (mobileMenuBtn) {
    mobileMenuBtn.addEventListener('click', function () {
      sidebar?.classList.add('mobile-open');
    });
  }

  if (overlay) {
    overlay.addEventListener('click', function () {
      sidebar?.classList.remove('mobile-open');
    });
  }

  // Close sidebar when a nav link is clicked on mobile
  const links = document.querySelectorAll('.sidebar__link');
  links.forEach(link => {
    link.addEventListener('click', function () {
      if (window.innerWidth <= 768) {
        sidebar?.classList.remove('mobile-open');
      }
    });
  });
}


// ---- Auto-initialize when the DOM is ready ----
// Pages can override the active page by setting: window.ACTIVE_PAGE = 'accounts';
document.addEventListener('DOMContentLoaded', function () {
  initSidebar(window.ACTIVE_PAGE || null);
});

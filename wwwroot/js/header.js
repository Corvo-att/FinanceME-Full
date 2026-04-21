/* ============================================================
   FINANCEME — Header Logic (header.js)

   In the MVC version, the header HTML is already rendered
   by the _Header.cshtml Razor partial. This script ONLY
   handles interactive behaviors (live clock, greeting, notifications).

   REQUIRES: utils.js must be loaded first!
   ============================================================ */


/**
 * Initialize the header / top bar.
 * The header markup is already in the DOM (rendered by _Header.cshtml).
 * We update the live clock and wire up the notification bell.
 *
 * @param {string} [userName] - the logged-in user's display name
 */
function initHeader(userName) {
  // --- 1. Update the live clock (runs every second) ---
  updateDateTime();
  setInterval(updateDateTime, 1000);

  // --- 2. Set up notification bell ---
  setupNotifications();
}


/**
 * Update the date/time display in the top bar.
 * Shows: "Friday, March 7, 2026 · 7:27 PM"
 */
function updateDateTime() {
  const dateEl = document.getElementById('current-date');
  if (!dateEl) return;

  const now = new Date();
  const dateStr = formatDate(now, 'full');
  const timeStr = formatDate(now, 'time');

  dateEl.textContent = `${dateStr} · ${timeStr}`;
}


/**
 * Set up the notification bell.
 * Hides the red dot when clicked.
 */
function setupNotifications() {
  const btn = document.getElementById('notification-btn');
  if (!btn) return;

  btn.addEventListener('click', function () {
    const dot = document.getElementById('notification-dot');
    if (dot) {
      dot.style.display = 'none';
    }
  });
}


// ---- Auto-initialize when the DOM is ready ----
document.addEventListener('DOMContentLoaded', function () {
  initHeader(window.USER_NAME || null);
});

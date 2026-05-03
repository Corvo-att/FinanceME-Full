/**
 * FinanceME — Budgets logic
 */

document.addEventListener('DOMContentLoaded', () => {

  const page = window.ACTIVE_PAGE;

  if (page === 'budgets') {
    // Animate progress bars based on data attributes
    document.querySelectorAll('.budget-card').forEach(card => {
      const spent = parseFloat(card.dataset.spent) || 0;
      const limit = parseFloat(card.dataset.limit) || 1;
      const pct = Math.min((spent / limit) * 100, 100);
      
      const fill = card.querySelector('.progress-fill');
      const label = card.querySelector('.progress-meta span:first-child');
      
      if (fill) {
        fill.style.width = '0%';
        setTimeout(() => fill.style.width = pct + '%', 50);
        
        // Remove existing classes
        fill.classList.remove('fill-safe', 'fill-warning', 'fill-danger');
        
        // Apply color based on percentage
        if (pct < 60) fill.classList.add('fill-safe');
        else if (pct < 85) fill.classList.add('fill-warning');
        else fill.classList.add('fill-danger');
      }
      
      if (label) {
          label.textContent = Math.round(pct) + '% used';
          label.className = '';
          if (pct < 60) label.classList.add('text-safe');
          else if (pct < 85) label.classList.add('text-warning');
          else label.classList.add('text-danger');
      }
    });
  }

  if (page === 'budgetdetail') {
    // Interactive progress trackers or specific drill-down logic
    const fill = document.querySelector('.progress-fill');
    if (fill) {
      const width = fill.style.width;
      fill.style.width = '0%';
      setTimeout(() => fill.style.width = width, 50);
    }
  }

  if (page === 'createbudget') {
    // Slider functionality, dynamically populating category limits, etc.
  }

});

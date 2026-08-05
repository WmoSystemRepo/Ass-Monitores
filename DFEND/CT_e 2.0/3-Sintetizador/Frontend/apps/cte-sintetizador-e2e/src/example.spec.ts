import { test, expect } from '@playwright/test';

test('overview carrega shell do monitor', async ({ page }) => {
  await page.goto('/');
  await expect(page.getByText('Monitor Sintetizador CT-e')).toBeVisible();
  await expect(page.getByRole('heading', { name: 'DFEND_CTe_Sintetizador' })).toBeVisible({
    timeout: 30000,
  });
});

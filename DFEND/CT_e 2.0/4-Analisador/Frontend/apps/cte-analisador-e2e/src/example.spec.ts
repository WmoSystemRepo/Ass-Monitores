import { test, expect } from '@playwright/test';

test('overview carrega shell do monitor', async ({ page }) => {
  await page.goto('/');
  await expect(page.getByText('Monitor Analisador CT-e')).toBeVisible();
  await expect(page.getByRole('heading', { name: 'DFEND_CTe_Analisador' })).toBeVisible({
    timeout: 30000,
  });
});

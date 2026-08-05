import { test, expect } from '@playwright/test';

test('overview carrega shell do monitor', async ({ page }) => {
  await page.goto('/');
  await expect(page.getByText('Monitor Integrador CT-e')).toBeVisible();
  await expect(page.getByRole('heading', { name: 'DFEND_CTe_Integrador' })).toBeVisible({
    timeout: 30000,
  });
});

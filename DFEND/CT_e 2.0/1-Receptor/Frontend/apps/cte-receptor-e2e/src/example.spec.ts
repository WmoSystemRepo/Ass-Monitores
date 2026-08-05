import { test, expect } from '@playwright/test';

test('overview carrega shell do monitor', async ({ page }) => {
  await page.goto('/');
  await expect(page.getByText('CT-e Receptor')).toBeVisible();
  await expect(page.getByRole('heading', { name: 'DFEND_CTe_Receptor' })).toBeVisible({
    timeout: 30000,
  });
});

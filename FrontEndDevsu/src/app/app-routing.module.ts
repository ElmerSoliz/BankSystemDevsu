import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LayoutComponent } from './components/layout/layout.component';

const routes: Routes = [
  {
    path: '',
    component: LayoutComponent,
    children: [
      {
        path: 'accounts',
        loadChildren: () => import('./features/account/account.module').then(m => m.AccountModule),
      },
      { path: 'clients', 
        loadChildren: () => import('./features/client/client.module').then(m => m.ClientModule) 
      },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

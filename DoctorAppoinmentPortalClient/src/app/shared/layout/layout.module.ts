import { NgModule } from "@angular/core";
import { NavbarComponent } from "./navbar/navbar.component";
import { FooterComponent } from "./footer/footer.component";
import { LayoutComponent } from "./layout.component";
import { SidebarComponent } from './sidebar/sidebar.component';
import { RouterModule } from "@angular/router";
import { CommonModule } from "@angular/common";

@NgModule({
    declarations:[
        LayoutComponent,
        NavbarComponent,
        FooterComponent,
        SidebarComponent
    ],
    imports:[
        RouterModule,
        CommonModule
    ]

})
export class LayoutModule{}
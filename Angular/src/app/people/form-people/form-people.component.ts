import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { PeopleService } from '../people.service';
import { AlertModalService } from 'src/app/shared/alert-modal.service';
import { Location } from '@angular/common';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-form-people',
  templateUrl: './form-people.component.html',
  styleUrls: ['./form-people.component.css']
})
export class FormPeopleComponent implements OnInit {

  form: FormGroup;
  submited: boolean = false;

  constructor(private fb: FormBuilder,
    private service: PeopleService,
    private modal: AlertModalService,
    private location: Location,
    private route: ActivatedRoute) { }

  ngOnInit() {

    const person = this.route.snapshot.data['person'];

    this.form = this.fb.group({
      rowKey: [person.rowKey],
      partitionKey: [person.partitionKey],
      name: [person.name, [Validators.required, Validators.minLength(3)]],
      email: [person.email, [Validators.required, Validators.minLength(3)]]
    });

  }

  hasError(field: string) {
    if (this.submited == true)
      return this.form.get(field).errors;
  }

  onSubmit() {
    this.submited = true;
    if (this.form.valid) {
      let msgSuccess = 'The new person was created.';
      let msgError = 'An error has occurred when you create a new person.';

      if (this.form.value.rowKey) {
        msgSuccess = 'The person was updated.';
        msgError = 'An error has occurred when you update a person.';
      }

      this.service.save(this.form.value).subscribe (
        success => {
          this.modal.showAlertSuccess(msgSuccess);
          setTimeout(()=> { this.location.back(); }, 3000);
        },
        error => this.modal.showAlertDanger(msgError)
      );
    }
  }

  onCancel() {
    this.submited = false;
    this.form.reset();
  }

}

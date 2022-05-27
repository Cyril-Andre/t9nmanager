import 'package:flutter/material.dart';
import 'package:flutter_holo_date_picker/date_picker.dart';
import 'package:flutter_holo_date_picker/i18n/date_picker_i18n.dart';
import 'package:intl/intl.dart';
import 'package:provider/provider.dart';
import 'package:t9n_manager_flutter_client/domains/authentication/apis/api_authentication.dart';
import 'package:t9n_manager_flutter_client/domains/authentication/models/user_registration_model.dart';

import '../../../generated/l10n.dart';
import '../../../shared/app_settings.dart';
import '../../../shared/app_state_notifier.dart';
import '../../../shared/widgets/screen_title.dart';
import '../../../shared/widgets/spacer.dart';

class SignupForm extends StatefulWidget {
  const SignupForm({Key? key}) : super(key: key);

  @override
  State<SignupForm> createState() => _SignupFormState();
}

class _SignupFormState extends State<SignupForm> {
  final _formKey = GlobalKey<FormState>();
  TextEditingController txtFirstname = TextEditingController();
  TextEditingController txtLastname = TextEditingController();
  TextEditingController txtLogin = TextEditingController();
  TextEditingController txtEmail = TextEditingController();
  TextEditingController txtPassword = TextEditingController();
  TextEditingController txtPasswordConfirm = TextEditingController();
  TextEditingController txtBirthdate = TextEditingController();
  bool _showPassword = false;
  bool _showPasswordConfirm = false;

  @override
  Widget build(BuildContext context) {
    AppSettings appSettings = context.watch<AppSettings>();
    double width = MediaQuery.of(context).size.width >
            context.watch<AppState>().screenMaxSize
        ? context.watch<AppState>().screenMaxSize
        : MediaQuery.of(context).size.width;
    double height = MediaQuery.of(context).size.height;
    return Center(
      child: SizedBox(
        width: .8 * width,
        height: height,
        child: Form(
          key: _formKey,
          autovalidateMode: AutovalidateMode.onUserInteraction,
          child: SingleChildScrollView(
            child: Column(
              mainAxisAlignment: MainAxisAlignment.start,
              crossAxisAlignment: CrossAxisAlignment.center,
              children: [
                verticalSpaceMedium,
                ScreenTitle(S.of(context).signup_form_title),
                Text(
                  S.of(context).signup_form_subtitle,
                  style: const TextStyle(
                      fontSize: 12, fontWeight: FontWeight.normal),
                ),
                verticalSpaceTiny,
                //lastname
                TextFormField(
                  validator: (String? value) {
                    if (value!.isEmpty) {
                      return S.of(context).signup_form_validator_lastname;
                    }
                    return null;
                  },
                  controller: txtLastname,
                  decoration: InputDecoration(
                    label: Text(S.of(context).signup_form_textfieldlabel_lastname),
                  ),
                ),
                verticalSpaceTiny,
                //firstname
                TextFormField(
                  validator: (String? value) {
                    if (value!.isEmpty) {
                      return S.of(context).signup_form_validator_firstname;
                    }
                    return null;
                  },
                  controller: txtFirstname,
                  decoration: InputDecoration(
                    label: Text(S.of(context).signup_form_textfieldlabel_firstname),
                  ),
                ),

                verticalSpaceTiny,
                //Login
                TextFormField(
                  validator: (String? value) {
                    if (value!.isEmpty) {
                      return S.of(context).signup_form_validator_login;
                    }
                    return null;
                  },
                  controller: txtLogin,
                  decoration: InputDecoration(
                    label: Text(S.of(context).signup_form_textfieldlabel_login),
                  ),
                ),
                verticalSpaceTiny,
                //Email
                TextFormField(
                  validator: (String? value) {
                    if (value!.isEmpty) {
                      return S.of(context).signup_form_validator_email;
                    }
                    return null;
                  },
                  controller: txtEmail,
                  keyboardType: TextInputType.emailAddress,
                  decoration: InputDecoration(
                    label: Text(S.of(context).signup_form_textfieldlabel_email),
                  ),
                ),
                verticalSpaceTiny,
                // Birthdate
                Container(
                  alignment: Alignment.center,
                  child: Row(
                      mainAxisAlignment: MainAxisAlignment.spaceBetween,
                      children: [
                        Expanded(
                          child: TextFormField(
                            readOnly: true,
                            controller: txtBirthdate,
                            decoration:
                                const InputDecoration(label: Text('Birthdate')),
                          ),
                        ),
                        SizedBox(
                          width: width * 0.2,
                          child: IconButton(
                              icon: const Icon(Icons.calendar_today),
                              onPressed: openDatePicker),
                        ),
                      ]),
                ),
                //Password
                TextFormField(
                  validator: (String? value) {
                    if (value == "") {
                      return S.of(context).signup_form_validator_password;
                    }
                    return null;
                  },
                  controller: txtPassword,
                  decoration: InputDecoration(
                      label: Text(
                          S.of(context).signup_form_textfieldlabel_password),
                      suffixIcon: IconButton(
                        onPressed: () {
                          setState(() {
                            _showPassword = !_showPassword;
                          });
                        },
                        icon: Icon(_showPassword
                            ? Icons.visibility_off
                            : Icons.visibility),
                      )),
                  obscureText: !_showPassword,
                ),
                verticalSpaceSmall,
                //Password confimation
                TextFormField(
                  validator: (String? value) {
                    if (value != txtPassword.text) {
                      return S
                          .of(context)
                          .signup_form_validator_password_confirm;
                    }
                    return null;
                  },
                  controller: txtPasswordConfirm,
                  decoration: InputDecoration(
                      label: Text(S
                          .of(context)
                          .signup_form_textfieldlabel_password_confirm),
                      suffixIcon: IconButton(
                        onPressed: () {
                          setState(() {
                            _showPasswordConfirm = !_showPasswordConfirm;
                          });
                        },
                        icon: Icon(_showPasswordConfirm
                            ? Icons.visibility_off
                            : Icons.visibility),
                      )),
                  obscureText: !_showPasswordConfirm,
                ),
                verticalSpaceSmall,
                ElevatedButton(
                    onPressed: () {
                      if (_formKey.currentState!.validate()) {
                        UserRegistrationModel userRegistrationModel =
                            UserRegistrationModel(                                
                                txtLogin.text,
                                txtEmail.text,
                                DateTime.parse(txtBirthdate.text),
                                txtPassword.text,
                                txtFirstname.text,
                                txtLastname.text
                                );
                        postSignup(userRegistrationModel, appSettings)
                            .then((value) {
                          if (value.httpStatus == 200) {
                            alert(S.of(context).signup_form_msg_signup_succeeded_title,value.message,S.of(context).common_button_ok)
                            .then((value) => Navigator.popAndPushNamed(context, '/'));
                          } 
                          else {
                            alert(S.of(context).signup_form_msg_signup_failed_title,value.message,S.of(context).common_button_ok);
                          }
                        });
                      }
                    },
                    child: Text(S.of(context).signup_form_button_label_signup)),
                verticalSpaceSmall,
                Text(S.of(context).signup_form_label_already_account),
                InkWell(
                  child: Text(S.of(context).signup_form_label_login,
                      style: const TextStyle(
                          decoration: TextDecoration.underline)),
                  onTap: () {
                    Navigator.popAndPushNamed(context, "/");
                  },
                ),
              ],
            ),
          ),
        ),
      ),
    );
  }

  void openDatePicker() async {
    var datePicked = await DatePicker.showSimpleDatePicker(
      context,
      initialDate: txtBirthdate.text != ''
          ? DateTime.parse(txtBirthdate.text)
          : DateTime(1994),
      firstDate: DateTime(1920),
      lastDate: DateTime.now(),
      dateFormat: "yyyy-MM-dd",
      locale: DateTimePickerLocale.en_us,
      looping: true,
    );
    if (datePicked != null) {
      txtBirthdate.text = DateFormat('yyyy-MM-dd').format(datePicked);
    }
  }

    Future<String?> alert(String title, String message, String buttonLabel) {
    return showDialog<String>(context: context, builder: (BuildContext context)=>
    AlertDialog(
      shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(15)),
      title: Text(title),
      content:Text(message),
      actions:<Widget>[
        TextButton(
          onPressed:()=> Navigator.pop(context,buttonLabel), 
          child: Text(buttonLabel)
        )
      ]
    )
    );
  }

}

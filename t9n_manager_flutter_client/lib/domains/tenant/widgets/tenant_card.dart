import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:t9n_manager_flutter_client/domains/tenant/apis/api_tenant.dart';
import 'package:t9n_manager_flutter_client/domains/tenant/models/invitation.dart';
import 'package:t9n_manager_flutter_client/shared/token_helper.dart';
import 'package:t9n_manager_flutter_client/shared/widgets/confirm_dialog.dart';
import 'package:t9n_manager_flutter_client/shared/widgets/input_dialog.dart';
import 'package:t9n_manager_flutter_client/shared/widgets/spacer.dart';

import '../../../generated/l10n.dart';
import '../../../shared/app_settings.dart';
import '../../../shared/app_state_notifier.dart';
import '../models/tenant.dart';

class TenantCard extends StatefulWidget {
  const TenantCard({Key? key, required this.tenant, required this.addNew, required this.refresh}) : super(key: key);
  final Tenant tenant;
  final bool addNew;
  final VoidCallback refresh;

  @override
  State<TenantCard> createState() => _TenantCardState();
}

class _TenantCardState extends State<TenantCard> {
  InputDialogController inputDialogController = InputDialogController();
  ConfirmDialogController confirmDialogController = ConfirmDialogController();

  @override
  Widget build(BuildContext context) {
    AppSettings appSettings = context.watch<AppSettings>();
    AppState appState = context.watch<AppState>();
    var jwt = appState.jwt;
    var currentUser = TokenHelper.parseJwt(jwt)['name'];
    bool isAdmin = currentUser == widget.tenant.adminUserName;
    Icon icon = const Icon(Icons.person_add);
    if (widget.addNew) {
      icon = const Icon(Icons.bookmark_add_outlined);
    } else {
      icon = const Icon(Icons.person_add);
    }
    double cardSize = context.watch<AppState>().cardSize;
    return SizedBox(
      width: cardSize,
      height: cardSize,
      child: GestureDetector(
        onTap: () {
          if (widget.tenant.tenantKey != "_") {
            changeTenant(widget.tenant, context);
          }
        },
        child: Card(
          elevation: 8.0,
          shadowColor: Colors.black87,
          margin: const EdgeInsets.all(8),
          child: Column(
            mainAxisSize: MainAxisSize.max,
            children: [
              isAdmin
                  ? const Icon(
                      Icons.bookmark_border,
                      size: 30.0,
                    )
                  : const Icon(
                      Icons.bookmark,
                      size: 30.0,
                    ),
              verticalSpaceRegular,
              Text(
                widget.tenant.tenantName,
                textAlign: TextAlign.center,
                style: const TextStyle(fontSize: 14),
              ),
              ButtonBar(
                alignment: MainAxisAlignment.end,
                mainAxisSize: MainAxisSize.min,
                buttonMinWidth: 4.0,
                buttonHeight: 4.0,
                buttonPadding: const EdgeInsets.all(1.0),
                children: [
                  if (widget.addNew)
                    InputDialog(
                        controller: inputDialogController,
                        callToActionIcon: icon,
                        title: S.of(context).tenant_card_add_tenant_title,
                        message: S.of(context).tenant_card_add_tenant_message,
                        toolTip: S.of(context).tenant_card_add_tenant_tooltip,
                        callBack: () async => await onAddTenant(appSettings, jwt, context)),
                  if (isAdmin)
                    InputDialog(
                        controller: inputDialogController,
                        callToActionIcon: icon,
                        title: S.of(context).tenant_card_add_user_title,
                        message: S.of(context).tenant_card_add_user_message,
                        toolTip: S.of(context).tenant_card_add_user_tooltip,
                        callBack: () => onAddUser(appSettings, jwt, context),
                        keyboardType: TextInputType.emailAddress
                    ),
                  if (!widget.addNew)
                    IconButton(
                        tooltip: S.of(context).tenant_card_select_tenant_tooltip,
                        onPressed: () {
                          changeTenant(widget.tenant, context);
                        },
                        icon: const Icon(Icons.check_box)),
                  if (!widget.addNew)
                    ConfirmDialog(
                        controller: confirmDialogController,
                        callToActionIcon: const Icon(Icons.delete),
                        title: S.of(context).tenant_card_leave_tenant_title,
                        message: S.of(context).tenant_card_leave_tenant_message,
                        toolTip: S.of(context).tenant_card_leave_tenant_tooltip,
                        callBack: () async => await onDelete(appSettings, jwt, context)),
                ],
              )
            ],
          ),
        ),
      ),
    );
  }

  changeTenant(Tenant tenant, BuildContext context) {
    context.read<AppState>().setTenant(tenant);
    Navigator.pop(context);
  }

  onDelete(AppSettings appSettings, String jwt, BuildContext context) async {
    await deleteTenant(appSettings, jwt, context, widget.tenant);
    widget.refresh();
  }

  onAddTenant(AppSettings appSettings, String jwt, BuildContext context) async {
    String tenantName = inputDialogController.value;
    await createTenant(appSettings, jwt, context, tenantName);
    widget.refresh();
  }

  onAddUser(AppSettings appSettings, String jwt, BuildContext context) {
    String userEmail = inputDialogController.value;
    Invitation invitation = Invitation(widget.tenant, userEmail);
    inviteUser(appSettings, jwt, context, invitation);
  }
}

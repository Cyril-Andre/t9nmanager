import 'package:flutter/material.dart';
import 'package:provider/provider.dart';
import 'package:t9n_manager_flutter_client/shared/widgets/spacer.dart';

import '../../../generated/l10n.dart';
import '../../../shared/app_state_notifier.dart';
import '../models/tenant.dart';

class TenantCard extends StatelessWidget {
  const TenantCard({Key? key, required this.tenant, required this.addNew}) : super(key: key);
  final Tenant tenant;
  final bool addNew;

  @override
  Widget build(BuildContext context) {
    Icon icon = const Icon(Icons.delete);
    if (addNew) {
      icon = const Icon(Icons.add);
    } else {
      icon = const Icon(Icons.delete);
    }
    double cardSize = context.watch<AppState>().cardSize;
    return SizedBox(
      width: cardSize,
      height: cardSize,
      child: GestureDetector(
        onTap: () {
          if (tenant.tenantKey != "_") {
            changeTenant(tenant, context);
          }
        },
        child: Card(
          elevation: 8.0,
          shadowColor: Colors.black87,
          margin: const EdgeInsets.all(8),
          child: Column(
            mainAxisSize: MainAxisSize.max,
            children: [
              const Icon(
                Icons.person,
                size: 30.0,
              ),
              verticalSpaceRegular,
              Text(
                tenant.tenantName,
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
                  IconButton(onPressed: () => pressButton(tenant, context), icon: icon),
                ],
              )
            ],
          ),
        ),
      ),
    );
  }

  Future<String?> alert(String title, String message, String confirmButtonLabel, String cancelButtonLabel, BuildContext context, Tenant tenant) async {
    return (showDialog<String>(
        context: context,
        builder: (BuildContext context) =>
            AlertDialog(shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(15)), title: Text(title), content: Text(message), actions: <Widget>[
              TextButton(onPressed: confirm(tenant, context), child: Text(confirmButtonLabel)),
              TextButton(onPressed: () => Navigator.pop(context, cancelButtonLabel), child: Text(cancelButtonLabel)),
            ])));
  }

  changeTenant(Tenant tenant, BuildContext context) {
    context.read<AppState>().setTenant(tenant);
    Navigator.pop(context);
  }

  pressButton(Tenant tenant, BuildContext context) {
    if (tenant.tenantKey == "_") {
      alert(S.of(context).tenant_alert_title_confirm_button_add, S.of(context).tenant_alert_message_confirm_button_add, S.of(context).common_button_yes,
          S.of(context).common_button_cancel, context, tenant);
    } else {
      alert(S.of(context).tenant_alert_title_confirm_button_delete, S.of(context).tenant_alert_message_confirm_button_delete, S.of(context).common_button_yes,
          S.of(context).common_button_cancel, context, tenant);
    }
  }

  confirm(Tenant tenant, BuildContext context) {
    if (tenant.tenantKey == "_") {
      Navigator.pushNamed(context, "/addTenant");
    }
    else {
      
    }
  }
}
